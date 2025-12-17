public class GoldOnKillRelic : Relic
{
  public override void Apply(Unit unit)
  {
    unit.OnKill += (target) => Player.instance.AddGold(1);
  }
}