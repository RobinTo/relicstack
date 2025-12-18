using UnityEngine;

[CreateAssetMenu(fileName = "New Relic", menuName = "Relic")]
public class RelicSO : ScriptableObject
{
  public string relicName;
  public string relicDescription;
  public GameObject prefab;
}