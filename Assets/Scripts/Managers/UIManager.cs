using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
  public static UIManager instance;
  public TMP_Text GoldText;

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
}