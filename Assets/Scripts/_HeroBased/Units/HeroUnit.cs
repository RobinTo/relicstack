using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class HeroUnitStats
{
  public float Health = 100f;
  public float Speed = 3.5f;
  public float AttackDamage = 10f;
  public float AttackRange = 2f;
  public float AttackCooldown = 1f;
  public TargetMode TargetMode;
}


[System.Serializable]
public class HeroUnitConfig
{
  public float AttackAnimationLength = 0.75f;
}

public enum HeroTargetMode
{
  Closest,
  HighestRange,
  LowestHealth,
  HighestHealth,
  IgnoreUnits, // Only for enemies

}

public enum HeroUnitState
{
  Not_started,
  Idle,
  Moving,
  Attacking,
  Casting,
  Dead,
}

public class HeroUnit : MonoBehaviour
{
  [SerializeField]
  HeroUnitState unitState = HeroUnitState.Idle;
  [SerializeField]
  public UnitStats Stats;
  [SerializeField]
  public HeroUnitConfig Config;

  [SerializeField]
  public Health Health;
  [SerializeField]
  public Mana Mana;

  [SerializeField]
  private NavMeshAgent agent;

  [SerializeField]
  private bool isEnemy;
  public bool IsEnemy => isEnemy;

  public HeroUnit CurrentTarget;

  public Action<HeroUnit> OnKill;
  private Animator animator;
  public Animator Animator => animator;

  private float lastTargetCheckTime;

  [Header("States")]
  public Casting castingState;
  public Attack attackState;

  void Start()
  {
    if (isEnemy)
    {
      HeroCombatManager.instance.EnemyUnits.Add(this);
    }
    else
    {
      HeroCombatManager.instance.PlayerUnits.Add(this);
    }
    Health.Initialize(Stats.Health);
    Health.OnTakeDamage += StartVisualDamageFeedback;
    Health.OnDeath += Die;
    animator = GetComponent<Animator>();
    StartCombat();
  }


  private void StartCombat()
  {
    FindNextCombatState();
  }

  public void GoToState(HeroUnitState newState)
  {
    unitState = newState;
  }

  public void FindNextCombatState()
  {

    if (CurrentTarget == null || Time.time - lastTargetCheckTime > 1f)
    {
      CurrentTarget = FindNewTarget();
    }

    if (CurrentTarget != null)
    {
      if (InRange())
      {
        if (Mana && Mana.FullMana)
        {
          Debug.Log("Full mana, casting spell");
          GoToState(HeroUnitState.Casting);
        }
        else if (attackState.IsReadyToAttack())
        {
          GoToState(HeroUnitState.Attacking);
        }
        else
        {
          GoToState(HeroUnitState.Idle);
        }
      }
      else
      {
        GoToState(HeroUnitState.Moving);
      }
    }
    else
    {
      GoToState(HeroUnitState.Idle);
    }
  }


  public void Update()
  {
    switch (unitState)
    {
      case HeroUnitState.Idle:
        UpdateIdle();
        break;
      case HeroUnitState.Moving:
        UpdateMoving();
        break;
      case HeroUnitState.Attacking:
        UpdateAttacking();
        break;
      case HeroUnitState.Casting:
        UpdateCasting();
        break;
    }
  }

  void UpdateIdle()
  {
    FindNextCombatState();
  }

  void UpdateMoving()
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
        FindNextCombatState();
      }
    }
  }

  void UpdateAttacking()
  {
    attackState.UpdateAttack();
  }

  void UpdateCasting()
  {
    castingState.UpdateCasting();
  }

  public void MoveTo(Vector3 destination)
  {
    agent.SetDestination(destination);
  }


  private void StartVisualDamageFeedback()
  {
    VisualFeedback();
  }

  private void Die()
  {
    Destroy(gameObject);
  }

  void OnDestroy()
  {
    if (isEnemy)
    {
      HeroCombatManager.instance.EnemyUnits.Remove(this);
    }
    else
    {
      HeroCombatManager.instance.PlayerUnits.Remove(this);
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

  private HeroUnit FindNewTarget()
  {
    List<HeroUnit> targetList = isEnemy ? HeroCombatManager.instance.PlayerUnits : HeroCombatManager.instance.EnemyUnits;

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

  private HeroUnit FindClosestTarget(List<HeroUnit> targetList)
  {
    float closestDistance = float.MaxValue;
    HeroUnit closestUnit = null;
    foreach (HeroUnit unit in targetList)
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

  private HeroUnit FindHighestRangeTarget(List<HeroUnit> targetList)
  {
    float highestRange = float.MinValue;
    HeroUnit targetUnit = null;
    foreach (HeroUnit unit in targetList)
    {
      if (unit.Stats.AttackRange > highestRange)
      {
        highestRange = unit.Stats.AttackRange;
        targetUnit = unit;
      }
    }
    return targetUnit;
  }

  private HeroUnit FindLowestHealthTarget(List<HeroUnit> targetList)
  {
    float lowestHealth = float.MaxValue;
    HeroUnit targetUnit = null;
    foreach (HeroUnit unit in targetList)
    {
      if (unit.Health.CurrentHealth < lowestHealth)
      {
        lowestHealth = unit.Health.CurrentHealth;
        targetUnit = unit;
      }
    }
    return targetUnit;
  }

  private HeroUnit FindHighestHealthTarget(List<HeroUnit> targetList)
  {
    float highestHealth = float.MinValue;
    HeroUnit targetUnit = null;
    foreach (HeroUnit unit in targetList)
    {
      if (unit.Health.CurrentHealth > highestHealth)
      {
        highestHealth = unit.Health.CurrentHealth;
        targetUnit = unit;
      }
    }
    return targetUnit;
  }

  public bool InRange()
  {
    return Vector3.Distance(transform.position, CurrentTarget.transform.position) <= Stats.AttackRange;
  }
}