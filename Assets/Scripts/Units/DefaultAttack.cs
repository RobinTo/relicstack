using System.Collections;
using UnityEngine;

public class DefaultAttack : MonoBehaviour
{
  public Unit me;
  private float attackTimer = 0f;

  void Start()
  {
    me = GetComponent<Unit>();
  }

  void Update()
  {
    attackTimer -= Time.deltaTime;
    if (me != null && me.CurrentTarget != null)
    {
      float distance = Vector3.Distance(me.transform.position, me.CurrentTarget.transform.position);
      if (distance <= me.Stats.AttackRange && attackTimer <= 0f)
      {
        transform.LookAt(me.CurrentTarget.transform);
        Attack(me.CurrentTarget);
        attackTimer = me.Stats.AttackCooldown;
      }
    }
  }

  private void Attack(Unit target)
  {
    if (target != null)
    {
      me.Animator.SetTrigger("Attack");
      StartCoroutine(DealDamageAfter(.25f, target, me.Stats.AttackDamage));
    }
  }

  IEnumerator DealDamageAfter(float time, Unit target, float damage)
  {
    yield return new WaitForSeconds(time);
    bool kill = target.TakeDamage(damage);
    if (kill)
    {
      me.OnKill?.Invoke(target);
    }
  }
}