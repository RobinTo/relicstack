using UnityEngine;

public class PlacerCollider : MonoBehaviour
{
  [SerializeField]
  CommanderController commander;

  void OnTriggerEnter(Collider other)
  {
    commander.HandlePlacerEnter(other);
  }

  void OnTriggerExit(Collider other)
  {
    commander.HandlePlacerExit(other);
  }
}
