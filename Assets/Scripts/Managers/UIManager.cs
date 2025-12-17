using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
  public static UIManager instance;
  public TMP_Text GoldText;

  [SerializeField]
  private WorldSpaceTooltip interactTooltip;

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
}