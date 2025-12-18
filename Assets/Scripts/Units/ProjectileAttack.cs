using UnityEngine;

public class ProjectileAttack : MonoBehaviour
{
  public Unit me;
  private float attackTimer = 0f;
  public Projectile Prefab;

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
      Projectile projectile = Instantiate(Prefab, me.transform.position + Vector3.up, Quaternion.identity);
      projectile.OwnerUnit = me;
      projectile.TargetUnit = target;
    }
  }
}