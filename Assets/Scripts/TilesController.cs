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

  private TileController GetTileController(GameObject obj) =>
    obj.GetComponentInChildren<TileController>();

  private Transform GetConnection(GameObject obj)
  {
    var controller = GetTileController(obj);
    return controller ? controller.connection : obj.transform;
  }

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
    var controller = GetTileController(newTile);
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

  private GameObject CreateAndPlaceTile(char letter, GameObject previousTile, bool shouldSetActive = true)
  {
    var tile = CreateTile(letter);
    PlaceTile(tile.transform, GetConnection(previousTile), shouldSetActive);
    return tile;
  }

  private void CreateTiles()
  {
    GameObject previousTile = gameObject;
    foreach (var letter in GetAlphabet())
      previousTile = CreateAndPlaceTile(letter, previousTile, false);
  }

  private void CreateWord(string word)
  {
    var previousTile = gameObject;
    foreach (var letter in word.ToUpper())
    {
      var tilesWithLetter = tilesByLetter[letter];
      var isTileFound = false;
      foreach (var tileWithLetter in tilesWithLetter)
      {
        if (tileWithLetter.activeInHierarchy) continue;

        PlaceTile(tileWithLetter.transform, GetConnection(previousTile));
        previousTile = tileWithLetter;
        isTileFound = true;
        break;
      }
      if (isTileFound) continue;

      Debug.Log($"Adding a tile for \'{letter}\'!");
      previousTile = CreateAndPlaceTile(letter, previousTile);
    }
  }

  void Start()
  {
    CreateTiles();
    CreateWord("pizza");
  }
}
