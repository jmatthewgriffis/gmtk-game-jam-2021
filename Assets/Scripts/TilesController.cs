using System.Collections.Generic;
using UnityEngine;

public class TilesController : MonoBehaviour
{
  private Dictionary<char, List<GameObject>> tilesByLetter =
    new Dictionary<char, List<GameObject>>();

  public GameObject tilePrefab;

  private string GetAlphabet()
  {
    char[] alphabet = {
      'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O',
      'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'
    };
    return new string(alphabet);
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

  private GameObject FindUnusedTile(char letter) =>
    tilesByLetter.ContainsKey(letter)
      ? tilesByLetter[letter].Find(tile => !tile.activeInHierarchy)
      : null;

  private GameObject CreateTile(char letter, bool shouldLog = false)
  {
    if (shouldLog) Debug.Log($"Adding a tile for \'{letter}\'!");
    var tile = Instantiate(tilePrefab);
    tile.name = letter.ToString();
    var controller = GetTileController(tile);
    controller.letter = letter;
    AddTileToTilesByLetter(letter, tile);
    return tile;
  }

  private GameObject GetTile(char letter, bool shouldLog = false)
  {
    var tile = FindUnusedTile(letter);
    return tile ? tile : CreateTile(letter, shouldLog);
  }

  private void PlaceTile(
    Transform tile,
    Transform position,
    bool shouldSetActive = true
  )
  {
    tile.SetParent(position);
    tile.localPosition = Vector3.zero;
    tile.SetParent(transform);
    tile.gameObject.SetActive(shouldSetActive);
  }

  private GameObject GetAndPlaceTile(
    char letter,
    GameObject previousTile,
    bool shouldLog = false,
    bool shouldSetActive = true
  )
  {
    var tile = GetTile(letter, shouldLog);
    PlaceTile(tile.transform, GetConnection(previousTile), shouldSetActive);
    return tile;
  }

  private void AssembleTiles(
    string letters,
    bool shouldLog = false,
    bool shouldSetActive = true
  )
  {
    var previousTile = gameObject;
    foreach (var letter in letters.ToUpper()) previousTile = GetAndPlaceTile(
      letter,
      previousTile,
      shouldLog,
      shouldSetActive
    );
  }

  void Start()
  {
    AssembleTiles(GetAlphabet(), false, false);
    AssembleTiles("pizza", true, true);
  }
}
