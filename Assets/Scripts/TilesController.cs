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
    obj ? obj.GetComponentInChildren<TileController>() : null;

  private Transform GetConnection(GameObject obj)
  {
    if (!obj) return null;

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
    GameObject previousTile,
    bool shouldSetActive = true
  )
  {
    var position = GetConnection(previousTile);
    tile.SetParent(position);
    tile.localPosition = Vector3.zero;

    var previousTileController = GetTileController(previousTile);
    if (previousTileController)
      tile.SetParent(previousTileController.GetParent());

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
    PlaceTile(tile.transform, previousTile, shouldSetActive);
    return tile;
  }

  private void AssembleTiles(
    string letters,
    GameObject container,
    bool shouldLog = false,
    bool shouldSetActive = true
  )
  {
    var previousTile = container;
    foreach (var letter in letters.ToUpper()) previousTile = GetAndPlaceTile(
      letter,
      previousTile,
      shouldLog,
      shouldSetActive
    );
  }

  public void AssembleTiles(string letters, GameObject container) =>
    AssembleTiles(letters, container, true, true);

  void Awake()
  {
    AssembleTiles(GetAlphabet(), gameObject, false, false);
    // AssembleTiles("pizza", null);
  }
}
