using System;
using UnityEngine;

public class Placer : MonoBehaviour
{

  [SerializeField]
  private Transform placePoint;

  IPickupable placedItem;

  public IPickupable PlacedItem => placedItem;

  public Action<GameObject> OnPlacedItemChanged;

  public void Place(IPickupable item)
  {
    if (placedItem != null)
    {
      Debug.LogWarning("Placer already has an item placed.");
      return;
    }

    Debug.Log("Placed item: " + item.PickupableName);
    GameObject itemGO = (item as MonoBehaviour).gameObject;
    itemGO.transform.position = placePoint.position;
    itemGO.transform.parent = null;
    placedItem = item;
    OnPlacedItemChanged?.Invoke(itemGO);
  }

  public void Remove(IPickupable item)
  {
    GameObject itemGO = (item as MonoBehaviour).gameObject;
    itemGO.transform.parent = placePoint;
    itemGO.transform.localPosition = Vector3.zero;
    placedItem = null;
    OnPlacedItemChanged?.Invoke(null);
  }
}