using UnityEngine;

public class GoldOnKillRelic : Relic
{
  public override void Apply(Unit unit)
  {
    Debug.Log("Applying GoldOnKillRelic to unit: " + unit.name);
    unit.OnKill += (target) => Player.instance.AddGold(1);
  }
}