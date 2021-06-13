using System.Collections.Generic;
using UnityEngine;

public class LettersController : MonoBehaviour
{
  private List<string> alphabet;
  private Dictionary<string, List<GameObject>> letters;

  public GameObject letter;

  private void SetAlphabet()
  {
    string[] _alphabet = {
      "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O",
      "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"
    };
    alphabet = new List<string>(_alphabet);
  }

  private GameObject CreateLetter(string text)
  {
    GameObject newLetter = Instantiate(letter);
    newLetter.name = text;
    LetterController controller = newLetter.GetComponentInChildren<LetterController>();
    controller.letter = text;
    return newLetter;
  }

  private void CreateLetters()
  {
    GameObject lastLetter = null;
    alphabet.ForEach(delegate (string _letter)
    {
      GameObject newLetter = CreateLetter(_letter);
      newLetter.transform.SetParent(lastLetter
        ? lastLetter.GetComponentInChildren<LetterController>().connection
        : transform);
      newLetter.transform.localPosition = Vector3.zero;
      lastLetter = newLetter;
    });
  }

  void Awake()
  {
    SetAlphabet();
  }

  void Start()
  {
    CreateLetters();
  }

  void Update()
  {

  }
}
