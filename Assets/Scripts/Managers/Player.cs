using UnityEngine;

public class Player : MonoBehaviour
{
  public static Player instance;
  private int gold = 0;
  public int Gold => gold;

  public void AddGold(int amount)
  {
    gold += amount;
    UIManager.instance.UpdateGold(gold);
  }

  public void SpendGold(int amount)
  {
    gold -= amount;
    UIManager.instance.UpdateGold(gold);
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