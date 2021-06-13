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

  private TileController GetController(GameObject tile) =>
    tile.GetComponentInChildren<TileController>();

  private Transform GetConnection(GameObject tile) =>
    GetController(tile).connection;

  private GameObject CreateTile(char letter)
  {
    var newTile = Instantiate(tile);
    newTile.name = letter.ToString();
    var controller = GetController(newTile);
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
        ? GetConnection(lastTile)
        : transform);
      newTile.transform.localPosition = Vector3.zero;
      newTile.transform.SetParent(transform);
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
        tileWithLetter.transform.SetParent(transform);
        tileWithLetter.SetActive(true);

        parent = GetConnection(tileWithLetter);
        isTileFound = true;
        break;
      }
      if (!isTileFound)
      {
        var newTile = CreateTile(letter);

        newTile.transform.SetParent(parent);
        newTile.transform.localPosition = Vector3.zero;
        newTile.transform.SetParent(transform);
        newTile.SetActive(true);

        tilesWithLetter.Add(newTile);
        Debug.Log($"Added a tile for \'{letter}\'!");
        parent = GetConnection(newTile);
      }
    }
  }

  void Start()
  {
    CreateTiles();
    CreateWord("pizza");
  }
}
