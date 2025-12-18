using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
  [SerializeField]
  private Unit prefab;
  [SerializeField]
  private Transform productionPoint;

  [SerializeField]
  private float productionInterval = 5f;
  private float productionTimer;
  private int unitsPer = 2;


  // Update is called once per frame
  void Update()
  {
    productionTimer += Time.deltaTime;
    if (productionTimer >= productionInterval)
    {
      for (int i = 0; i < unitsPer; i++)
      {
        ProduceUnit();
      }
      productionTimer = 0f;

      productionInterval = Mathf.Max(1f, productionInterval - 0.2f);
      if (productionInterval < 2.5f)
      {
        unitsPer += 1;
        productionInterval = 5f;
      }
    }
  }

  private void ProduceUnit()
  {
    Vector3 point = productionPoint.position;
    point += Random.insideUnitSphere * 1.5f;
    point.y = productionPoint.position.y;
    Instantiate(prefab, point, Quaternion.identity);
  }
}
