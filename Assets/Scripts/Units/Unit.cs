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
}

public class Unit : MonoBehaviour
{
  [SerializeField]
  public UnitStats Stats;

  [SerializeField]
  private NavMeshAgent agent;

  [SerializeField]
  private bool isEnemy;

  public Unit CurrentTarget;

  private float MaxHealth;
  private float CurrentHealth;

  public Action<Unit> OnKill;

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
  }

  void Update()
  {
    if (CurrentTarget != null)
    {
      if (Vector3.Distance(transform.position, CurrentTarget.transform.position) > Stats.AttackRange)
      {
        agent.isStopped = false;
        agent.SetDestination(CurrentTarget.transform.position);
      }
      else
      {
        agent.isStopped = true;
      }
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

      CurrentTarget = closestUnit;
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
    if (isEnemy)
    {
      GameManager.instance.EnemyUnits.Remove(this);
    }
    else
    {
      GameManager.instance.PlayerUnits.Remove(this);
    }
    Destroy(gameObject);
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
}