using UnityEngine;
using UnityEngine.UI;

public class TileController : MonoBehaviour
{
  public string letter;
  public Text text;
  public Transform connection;

  private void SetText(string val)
  {
    text.text = val;
  }

  void Start()
  {
    SetText(letter);
  }
}
