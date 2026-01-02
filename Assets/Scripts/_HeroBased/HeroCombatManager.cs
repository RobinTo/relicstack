using System.Collections.Generic;
using UnityEngine;

public class HeroCombatManager : MonoBehaviour
{
  public static HeroCombatManager instance;

  public List<HeroUnit> PlayerUnits { get; private set; } = new List<HeroUnit>();
  public List<HeroUnit> EnemyUnits { get; private set; } = new List<HeroUnit>();

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