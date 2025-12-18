using System.Collections.Generic;
using UnityEngine;

public class UnitProducer : MonoBehaviour, IInteractable
{
  [SerializeField]
  private Unit prefab;
  [SerializeField]
  private Transform productionPoint;

  [SerializeField]
  private float productionInterval = 5f;
  private float productionTimer;

  [SerializeField]
  private List<Placer> relicPlacers = new List<Placer>();
  private List<Relic> relics = new List<Relic>();

  public string InteractableName => "Unit Producer";
  public string InteractableDescription => "Produces units at regular intervals.";

  // Start is called once before the first execution of Update after the MonoBehaviour is created
  void Start()
  {
    foreach (Placer placer in relicPlacers)
    {
      placer.OnPlacedItemChanged += (GameObject itemGO) =>
      {
        UpdateRelics();
      };
    }
    UpdateRelics();
  }

  private void UpdateRelics()
  {
    relics.Clear();
    foreach (Placer placer in relicPlacers)
    {
      if (placer.PlacedItem == null)
      {
        continue;
      }
      GameObject go = (placer.PlacedItem as MonoBehaviour).gameObject;
      Relic relic = go.GetComponent<Relic>();
      if (relic != null)
      {
        relics.Add(relic);
      }
    }
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

  public void Interact(GameObject interactor)
  {
    Debug.Log("Interacted with Unit Producer");
  }
}
