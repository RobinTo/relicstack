using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
  public static UIManager instance;
  public TMP_Text GoldText;

  [SerializeField]
  private WorldSpaceTooltip interactTooltip;

  [SerializeField]
  private WorldSpaceTooltip pickupTooltip;

  [SerializeField]
  public GameObject choicePanel;

  [SerializeField]
  private Choice choicePrefab;


  public void UpdateGold(int amount)
  {
    GoldText.text = "Gold: " + amount.ToString();
  }

  void Awake()
  {
    if (instance == null)
    {
      instance = this;
    }
    else
    {
      Destroy(gameObject);
    }
  }

  public void ShowInteractPrompt(string name, string description, Vector3 position)
  {
    interactTooltip.ShowTooltip(name, description, position);
  }

  public void HideInteractPrompt()
  {
    interactTooltip.HideTooltip();
  }

  public void ShowPickupPrompt(string name, string description, Vector3 position)
  {
    pickupTooltip.ShowTooltip(name, description, position);
  }

  public void HidePickupPrompt()
  {
    pickupTooltip.HideTooltip();
  }

  public void ShowChoicePanel(List<ChoiceData> choices)
  {
    foreach (Transform child in choicePanel.transform)
    {
      Destroy(child.gameObject);
    }

    foreach (ChoiceData choiceData in choices)
    {
      Choice choiceInstance = Instantiate(choicePrefab, choicePanel.transform);
      choiceInstance.InitializeChoice(choiceData.Text, choiceData.Action);
    }

    choicePanel.SetActive(true);
    InputManager.Instance.SetUIActionMapActive();

    var firstButton = choicePanel.GetComponentInChildren<Button>();

    if (firstButton != null)
    {
      EventSystem.current.SetSelectedGameObject(firstButton.gameObject);
    }
  }

  public void HideChoicePanel()
  {
    choicePanel.SetActive(false);
  }
}