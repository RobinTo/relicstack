using UnityEngine;

public class Projectile : MonoBehaviour
{
  public Unit OwnerUnit { get; set; }
  public Unit TargetUnit { get; set; }

  public float Speed = 10f;

  void Update()
  {
    if (TargetUnit == null)
    {
      Destroy(gameObject);
      return;
    }

    Vector3 targetPosition = TargetUnit.transform.position + Vector3.up;
    Vector3 direction = (targetPosition - transform.position).normalized;
    transform.position += direction * Speed * Time.deltaTime;

    float distanceToTarget = Vector3.Distance(transform.position, targetPosition);
    if (distanceToTarget < 0.2f)
    {
      bool kill = TargetUnit.TakeDamage(OwnerUnit.Stats.AttackDamage);
      if (kill)
      {
        OwnerUnit.OnKill?.Invoke(TargetUnit);
      }
      Destroy(gameObject);
    }
  }
}