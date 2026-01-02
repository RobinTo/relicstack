using System;
using UnityEngine;

public class Health : MonoBehaviour
{
  public float CurrentHealth;
  public float MaxHealth;

  bool dead = false;

  public Action OnDeath;
  public Action OnTakeDamage;

  public void Initialize(float maxHealth)
  {
    MaxHealth = maxHealth;
    CurrentHealth = MaxHealth;
  }

  public bool TakeDamage(float damage)
  {
    if (dead) return false;
    CurrentHealth -= damage;
    OnTakeDamage?.Invoke();
    Debug.Log($"Unit took {damage} damage and has {CurrentHealth} health remaining.");
    if (CurrentHealth <= 0)
    {
      CurrentHealth = 0;
      dead = true;
      OnDeath?.Invoke();
      return true;
    }
    return false;
  }

  public void Heal(float amount)
  {
    if (dead) return;
    CurrentHealth += amount;
    if (CurrentHealth > MaxHealth)
    {
      CurrentHealth = MaxHealth;
    }
  }
}