using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
  public static GameManager instance;

  public List<Unit> PlayerUnits { get; private set; } = new List<Unit>();
  public List<Unit> EnemyUnits { get; private set; } = new List<Unit>();

  private void Awake()
  {
    if (instance == null)
    {
      instance = this;
    }
    else
    {
      Destroy(gameObject);
    }
  }
}