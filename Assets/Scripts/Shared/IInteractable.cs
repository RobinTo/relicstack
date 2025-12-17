using UnityEngine;

public interface IInteractable
{
  public string InteractableName { get; }
  public string InteractableDescription { get; }
  void Interact(GameObject interactor);
}