using UnityEngine;
using UnityEngine.UI;

public class TileController : MonoBehaviour
{
  public char letter;
  public Text text;
  public Transform connection;

  private void SetText(char val)
  {
    text.text = val.ToString();
  }

  void Start()
  {
    SetText(letter);
  }
}
