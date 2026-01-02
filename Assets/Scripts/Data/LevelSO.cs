using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyTiming
{
  public float TimeStamp;
  public GameObject enemy;
}

[System.Serializable]
public class Wave
{
  public List<EnemyTiming> EnemyTiming;
}

[CreateAssetMenu(fileName = "New Level", menuName = "Level")]
public class LevelSO : ScriptableObject
{
  public List<Wave> waves;
}