using System;
using System.Collections;
using UnityEngine;

public class HeroProjectileAttack : MonoBehaviour, IAttack
{
  public HeroUnit owner;

  private Coroutine attackCoroutine;

  public Action OnAttackHit;
  [SerializeField]
  private HeroProjectile projectile;

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
    float timePerAttack = owner.Config.AttackAnimationLength / 2;
    owner.Animator.SetTrigger("Attack");
    while (timer < timePerAttack)
    {
      timer += Time.deltaTime;
      yield return null;
    }
    LastAttackTime = Time.time;
    HeroProjectile newProj = Instantiate(projectile, transform.position + Vector3.up, Quaternion.identity);
    newProj.Owner = owner;
    newProj.TargetAtCastTime = target;
    while (timer < timePerAttack)
    {
      timer += Time.deltaTime;
      yield return null;
    }

    attackCoroutine = null;
    owner.FindNextCombatState();
  }
}