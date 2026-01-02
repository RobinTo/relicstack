using UnityEngine;

public class Mana : MonoBehaviour
{
  public HeroUnit owner;

  public int CurrentMana { get; private set; }
  public int MaxMana { get; private set; }
  public int StartingMana = 0;

  private void Awake()
  {
    MaxMana = owner.Stats.MaxMana;
    CurrentMana = StartingMana;
  }

  void Start()
  {
    owner.attackState.OnAttackHit += () => AddMana(10);
  }

  public bool FullMana => CurrentMana >= MaxMana;

  public void SetMana(int amount)
  {
    CurrentMana = Mathf.Clamp(amount, 0, MaxMana);
  }

  public void AddMana(int amount)
  {
    SetMana(CurrentMana + amount);
  }

  public void ConsumeMana(int amount)
  {
    SetMana(CurrentMana - amount);
  }

  public void ConsumeAllMana()
  {
    CurrentMana = 0;
  }
}