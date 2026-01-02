using System;
using System.Collections;
using UnityEngine;

public interface IAttack
{
  public void UpdateAttack();
  public bool IsReadyToAttack();
}

public class Attack : MonoBehaviour, IAttack
{
  public HeroUnit owner;

  private Coroutine attackCoroutine;

  public Action OnAttackHit;
  private float LastAttackTime;

  public void UpdateAttack()
  {

    if (attackCoroutine == null)
    {
      attackCoroutine = StartCoroutine(PerformAttack(owner.CurrentTarget));
    }
  }

  public bool IsReadyToAttack()
  {
    return attackCoroutine == null && LastAttackTime <= Time.time - owner.Stats.AttackCooldown;
  }


  public IEnumerator PerformAttack(HeroUnit target)
  {
    float timer = 0;
    // Asuming that attack should trigger at animationlength/2
    float timePerAttack = owner.Config.AttackAnimationLength / 2f;
    owner.Animator.SetTrigger("Attack");
    while (timer < timePerAttack)
    {
      timer += Time.deltaTime;
      yield return null;
    }
    LastAttackTime = Time.time;
    target.Health.TakeDamage(owner.Stats.AttackDamage);
    OnAttackHit?.Invoke();
    timer = 0;
    while (timer < timePerAttack)
    {
      timer += Time.deltaTime;
      yield return null;
    }
    attackCoroutine = null;
    owner.FindNextCombatState();
  }
}