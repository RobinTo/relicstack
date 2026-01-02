using System;
using System.Collections;
using UnityEngine;

public class Casting : MonoBehaviour
{
  public HeroUnit owner;

  [SerializeField]
  Spell spell;

  private Coroutine castingCoroutine;
  public Action OnCast;

  public void UpdateCasting()
  {
    if (castingCoroutine == null)
    {
      castingCoroutine = StartCoroutine(PerformCasting());
      owner.Mana.ConsumeAllMana();
      Debug.Log("All mana consumed and cast started");
    }
  }

  public IEnumerator PerformCasting()
  {
    float castingTime = 1.5f;
    float timer = 0;
    owner.Animator.SetBool("Casting", true);
    while (timer < castingTime)
    {
      timer += Time.deltaTime;
      yield return null;
    }

    owner.Animator.SetBool("Casting", false);
    Debug.Log("Casting spell: " + spell.name);
    spell.CastSpell();

    OnCast?.Invoke();
    castingCoroutine = null;
    owner.FindNextCombatState();
  }
}