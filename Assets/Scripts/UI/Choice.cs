using TMPro;
using UnityEngine;
using UnityEngine.UI;

public struct ChoiceData
{
  public string Text;
  public Choice.ChoiceAction Action;

  public ChoiceData(string text, Choice.ChoiceAction action)
  {
    Text = text;
    Action = action;
  }
}

public class Choice : MonoBehaviour
{
  [SerializeField]
  TMP_Text ChoiceText;
  [SerializeField]
  Button ChoiceButton;

  public delegate void ChoiceAction();


  public void InitializeChoice(string text, ChoiceAction action)
  {
    ChoiceText.text = text;
    ChoiceButton.onClick.AddListener(() => action());
  }
}