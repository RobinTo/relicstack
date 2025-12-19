using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class UnitStats
{
  public float Health = 100f;
  public float Speed = 3.5f;
  public float AttackDamage = 10f;
  public float AttackRange = 2f;
  public float AttackCooldown = 1f;
  public TargetMode TargetMode;
}

public enum TargetMode
{
  Closest,
  HighestRange,
  LowestHealth,
  HighestHealth,
  IgnoreUnits, // Only for enemies

}

public class Unit : MonoBehaviour
{
  [SerializeField]
  public UnitStats Stats;

  [SerializeField]
  private NavMeshAgent agent;

  [SerializeField]
  private bool isEnemy;
  public bool IsEnemy => isEnemy;

  public Unit CurrentTarget;

  private float MaxHealth;
  private float CurrentHealth;

  public Action<Unit> OnKill;
  private Animator animator;
  public Animator Animator => animator;

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
    MaxHealth = Stats.Health;
    CurrentHealth = MaxHealth;
    animator = GetComponent<Animator>();
  }

  void Update()
  {
    if (CurrentTarget != null)
    {
      if (Vector3.Distance(transform.position, CurrentTarget.transform.position) > Stats.AttackRange)
      {
        agent.isStopped = false;
        agent.SetDestination(CurrentTarget.transform.position);
        animator.SetBool("Walking", true);
      }
      else
      {
        agent.isStopped = true;
        animator.SetBool("Walking", false);
      }
    }
    else
    {
      CurrentTarget = FindNewTarget();
    }

    if (isEnemy && CurrentTarget == null && Headquarters.instance != null)
    {
      agent.isStopped = false;
      agent.SetDestination(Headquarters.instance.transform.position);
    }
  }

  public void MoveTo(Vector3 destination)
  {
    agent.SetDestination(destination);
  }

  public bool TakeDamage(float damage)
  {
    if (CurrentHealth <= 0)
      return false;

    VisualFeedback();
    CurrentHealth -= damage;
    if (CurrentHealth <= 0)
    {
      Die();
      return true;
    }
    return false;
  }

  private void Die()
  {
    Destroy(gameObject);
  }

  void OnDestroy()
  {
    if (isEnemy)
    {
      GameManager.instance.EnemyUnits.Remove(this);
    }
    else
    {
      GameManager.instance.PlayerUnits.Remove(this);
    }
  }


  // Temporary just for debug effect
  private Coroutine visualFeedbackCoroutine;
  Color originalColor;
  private bool colorSet = false;
  private void VisualFeedback()
  {
    if (visualFeedbackCoroutine != null)
    {
      StopCoroutine(visualFeedbackCoroutine);
    }
    visualFeedbackCoroutine = StartCoroutine(VisualFeedbackCoroutine());
  }

  IEnumerator VisualFeedbackCoroutine()
  {
    Renderer renderer = GetComponentInChildren<Renderer>();
    if (!colorSet)
    {
      originalColor = renderer.material.color;
      colorSet = true;
    }
    renderer.material.color = Color.red;
    yield return new WaitForSeconds(0.15f);
    renderer.material.color = originalColor;
  }

  void OnDrawGizmos()
  {
    Gizmos.color = Color.red;
    if (CurrentTarget != null)
      Gizmos.DrawLine(transform.position + Vector3.up * 0.5f, CurrentTarget.transform.position + Vector3.up * 0.5f);
  }

  private Unit FindNewTarget()
  {
    List<Unit> targetList = isEnemy ? GameManager.instance.PlayerUnits : GameManager.instance.EnemyUnits;

    if (targetList.Count == 0)
      return null;

    switch (Stats.TargetMode)
    {
      case TargetMode.Closest:
        return FindClosestTarget(targetList);

      case TargetMode.HighestRange:
        return FindHighestRangeTarget(targetList);

      case TargetMode.LowestHealth:
        return FindLowestHealthTarget(targetList);

      case TargetMode.HighestHealth:
        return FindHighestHealthTarget(targetList);

      case TargetMode.IgnoreUnits:
        return null;

      default:
        return FindClosestTarget(targetList);
    }
  }

  private Unit FindClosestTarget(List<Unit> targetList)
  {
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
    return closestUnit;
  }

  private Unit FindHighestRangeTarget(List<Unit> targetList)
  {
    float highestRange = float.MinValue;
    Unit targetUnit = null;
    foreach (Unit unit in targetList)
    {
      if (unit.Stats.AttackRange > highestRange)
      {
        highestRange = unit.Stats.AttackRange;
        targetUnit = unit;
      }
    }
    return targetUnit;
  }

  private Unit FindLowestHealthTarget(List<Unit> targetList)
  {
    float lowestHealth = float.MaxValue;
    Unit targetUnit = null;
    foreach (Unit unit in targetList)
    {
      if (unit.CurrentHealth < lowestHealth)
      {
        lowestHealth = unit.CurrentHealth;
        targetUnit = unit;
      }
    }
    return targetUnit;
  }

  private Unit FindHighestHealthTarget(List<Unit> targetList)
  {
    float highestHealth = float.MinValue;
    Unit targetUnit = null;
    foreach (Unit unit in targetList)
    {
      if (unit.CurrentHealth > highestHealth)
      {
        highestHealth = unit.CurrentHealth;
        targetUnit = unit;
      }
    }
    return targetUnit;
  }
}