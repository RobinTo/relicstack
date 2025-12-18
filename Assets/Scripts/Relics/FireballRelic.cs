using Unity.VisualScripting;
using UnityEngine;

public class FireballRelic : Relic
{
  [SerializeField]
  private Projectile fireballPrefab;

  public override void Apply(Unit unit)
  {
    DefaultAttack attack = unit.GetComponent<DefaultAttack>();
    if (attack != null)
    {
      Destroy(attack);
    }
    ProjectileAttack newAttack = unit.AddComponent<ProjectileAttack>();
    newAttack.Prefab = fireballPrefab;
    // Minimum attack range of 5 units, but add 3 if already ranged.
    unit.Stats.AttackRange = Mathf.Max(unit.Stats.AttackRange + 3, 5);
  }
}