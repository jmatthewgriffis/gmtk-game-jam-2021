using System.Collections.Generic;
using UnityEngine;

public class WordsController : MonoBehaviour
{
  public List<Word> words = new List<Word>();
  public TilesController tilesController;

  void Start()
  {
    foreach (var word in words)
      tilesController.AssembleTiles(word.letters, word.container);
  }
}
