using System.Collections.Generic;
using UnityEngine;

public class UnitProducer : MonoBehaviour
{
  [SerializeField]
  private Unit prefab;
  [SerializeField]
  private Transform productionPoint;

  [SerializeField]
  private float productionInterval = 5f;
  private float productionTimer;

  private List<Relic> relics = new List<Relic>();

  // Start is called once before the first execution of Update after the MonoBehaviour is created
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {
    productionInterval -= Time.deltaTime;
    if (productionInterval <= 0f)
    {
      ProduceUnit();
      productionInterval = 5f; // Reset the timer
    }
  }

  private void ProduceUnit()
  {
    Unit unit = Instantiate(prefab, productionPoint.position, Quaternion.identity);
    foreach (Relic relic in relics)
    {
      relic.Apply(unit);
    }
  }
}
