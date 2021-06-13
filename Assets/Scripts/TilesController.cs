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

  private void AddTileToTilesByLetter(char letter, GameObject tile)
  {
    if (!tilesByLetter.ContainsKey(letter))
      tilesByLetter.Add(letter, new List<GameObject>());

    tilesByLetter[letter].Add(tile);
  }

  private GameObject CreateTile(char letter)
  {
    var newTile = Instantiate(tile);
    newTile.name = letter.ToString();
    var controller = GetController(newTile);
    controller.letter = letter;
    AddTileToTilesByLetter(letter, newTile);
    return newTile;
  }

  private void PlaceTile(Transform tile, Transform position, bool shouldSetActive = true)
  {
    tile.SetParent(position);
    tile.localPosition = Vector3.zero;
    tile.SetParent(transform);
    tile.gameObject.SetActive(shouldSetActive);
  }

  private void CreateTiles()
  {
    GameObject lastTile = null;
    foreach (var letter in GetAlphabet())
    {
      var newTile = CreateTile(letter);
      PlaceTile(newTile.transform, lastTile ? GetConnection(lastTile) : transform, false);
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

        PlaceTile(tileWithLetter.transform, parent);
        parent = GetConnection(tileWithLetter);
        isTileFound = true;
        break;
      }
      if (!isTileFound)
      {
        Debug.Log($"Adding a tile for \'{letter}\'!");
        var newTile = CreateTile(letter);
        PlaceTile(newTile.transform, parent);
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
