using UnityEngine;

public class HeroPlacerCollider : MonoBehaviour
{
  [SerializeField]
  HeroCommanderController commander;

  void OnTriggerEnter(Collider other)
  {
    commander.HandlePlacerEnter(other);
  }

  void OnTriggerExit(Collider other)
  {
    commander.HandlePlacerExit(other);
  }
}
