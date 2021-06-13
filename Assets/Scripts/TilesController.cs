using System.Collections.Generic;
using UnityEngine;

public class TilesController : MonoBehaviour
{
  private Dictionary<char, List<GameObject>> tilesByLetter = new Dictionary<char, List<GameObject>>();

  public GameObject tile;

  private List<char> GetAlphabet()
  {
    char[] alphabet = {
      'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O',
      'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'
    };
    return new List<char>(alphabet);
  }

  private GameObject CreateTile(char letter)
  {
    var newTile = Instantiate(tile);
    newTile.name = letter.ToString();
    var controller = newTile.GetComponentInChildren<TileController>();
    controller.letter = letter;
    return newTile;
  }

  private void CreateTiles()
  {
    GameObject lastTile = null;
    foreach (var letter in GetAlphabet())
    {
      var newTile = CreateTile(letter);
      newTile.transform.SetParent(lastTile
        ? lastTile.GetComponentInChildren<TileController>().connection
        : transform);
      newTile.transform.localPosition = Vector3.zero;
      newTile.SetActive(false);
      tilesByLetter.Add(letter, new List<GameObject> { newTile });
      lastTile = newTile;
    }
  }

  private void CreateWord(string word)
  {
    var parent = transform;
    foreach (var letter in word.ToUpper())
    {
      var tilesWithLetter = tilesByLetter[letter];
      var isTileFound = false;
      foreach (var tileWithLetter in tilesWithLetter)
      {
        if (tileWithLetter.activeInHierarchy) continue;

        tileWithLetter.transform.SetParent(parent);
        if (parent == transform) tileWithLetter.transform.position = Vector3.zero;
        else tileWithLetter.transform.localPosition = Vector3.zero;
        tileWithLetter.SetActive(true);

        parent = tileWithLetter.GetComponentInChildren<TileController>().connection;
        isTileFound = true;
        break;
      }
      if (!isTileFound)
      {
        var newTile = CreateTile(letter);
        newTile.transform.SetParent(parent);
        newTile.transform.localPosition = Vector3.zero;
        tilesWithLetter.Add(newTile);
        Debug.Log($"Added a tile for \'{letter}\'!");
      }
    }
  }

  void Start()
  {
    CreateTiles();
    CreateWord("pizza");
  }
}
