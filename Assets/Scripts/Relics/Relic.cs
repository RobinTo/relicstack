using UnityEngine;

public class Relic : MonoBehaviour, IPickupable
{
  public string PickupableName => relicName;
  public string PickupableDescription => relicDescription;

  [SerializeField]
  private string relicName;

  [SerializeField]
  private string relicDescription;

  public virtual void Apply(Unit unit)
  {

  }
}