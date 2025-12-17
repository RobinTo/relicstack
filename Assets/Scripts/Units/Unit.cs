using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour
{
  [SerializeField]
  private NavMeshAgent agent;

  [SerializeField]
  private bool isEnemy;

  Unit currentTarget;

  void Start()
  {
    if (isEnemy)
    {
      GameManager.instance.EnemyUnits.Add(this);
    }
    else
    {
      GameManager.instance.PlayerUnits.Add(this);
    }
  }

  void Update()
  {
    if (currentTarget != null)
    {
      agent.SetDestination(currentTarget.transform.position);
    }
    else
    {
      List<Unit> targetList = isEnemy ? GameManager.instance.PlayerUnits : GameManager.instance.EnemyUnits;
      float closestDistance = float.MaxValue;
      Unit closestUnit = null;
      foreach (Unit unit in targetList)
      {
        float distance = Vector3.Distance(transform.position, unit.transform.position);
        if (distance < closestDistance)
        {
          closestDistance = distance;
          closestUnit = unit;
        }
      }

      currentTarget = closestUnit;
    }
  }

  public void MoveTo(Vector3 destination)
  {
    agent.SetDestination(destination);
  }
}