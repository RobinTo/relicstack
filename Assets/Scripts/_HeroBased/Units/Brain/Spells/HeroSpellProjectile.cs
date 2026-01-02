using UnityEngine;
using UnityEngine.Pool;

public class HeroSpellProjectile : MonoBehaviour, ISpellInstanceWithOwner
{
  public HeroUnit Owner { get; set; }
  public HeroUnit TargetAtCastTime { get; set; }
  public SpawnGameObjectSpell OwningSpell { get; set; }

  [SerializeField]
  private float speed = 10f;

  public void OnStart() { }

  private void Update()
  {
    if (TargetAtCastTime == null)
    {
      Destroy(gameObject);
      return;
    }
    else
    {
      Vector3 direction = (TargetAtCastTime.transform.position - transform.position).normalized;
      transform.position += direction * speed * Time.deltaTime;

      float distanceToTarget = Vector3.Distance(transform.position, TargetAtCastTime.transform.position);
      if (distanceToTarget < 0.1f)
      {
        bool kill = TargetAtCastTime.Health.TakeDamage(Owner.Stats.AttackDamage);
        if (kill)
        {
          Owner.OnKill?.Invoke(TargetAtCastTime);
        }
        OwningSpell.ReleaseSpell(this);
      }

    }
  }
}