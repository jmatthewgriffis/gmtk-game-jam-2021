using System.Collections.Generic;
using UnityEngine;

public class TilesController : MonoBehaviour
{
  private Dictionary<string, List<GameObject>> tilesByLetter;

  public GameObject tile;

  private List<string> GetAlphabet()
  {
    string[] alphabet = {
      "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O",
      "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"
    };
    return new List<string>(alphabet);
  }

  private GameObject CreateTile(string letter)
  {
    GameObject newTile = Instantiate(tile);
    newTile.name = letter;
    TileController controller = newTile.GetComponentInChildren<TileController>();
    controller.letter = letter;
    return newTile;
  }

  private void CreateTiles()
  {
    GameObject lastTile = null;
    GetAlphabet().ForEach(delegate (string letter)
    {
      GameObject newTile = CreateTile(letter);
      newTile.transform.SetParent(lastTile
        ? lastTile.GetComponentInChildren<TileController>().connection
        : transform);
      newTile.transform.localPosition = Vector3.zero;
      lastTile = newTile;
    });
  }

  void Start()
  {
    CreateTiles();
  }
}
