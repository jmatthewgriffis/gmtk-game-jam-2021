using UnityEngine;
using UnityEngine.UI;

public class LetterController : MonoBehaviour
{
  public Text text;

  public string letter = "Z";

  void Start()
  {
    SetText(letter);
  }

  void Update()
  {

  }

  private void SetText(string val)
  {
    if (!text) return;
    text.text = val;
  }
}
