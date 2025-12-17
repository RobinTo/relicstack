using System.Collections.Generic;
using UnityEngine;

public class CommanderController : MonoBehaviour
{
  Rigidbody rb;
  InputSystem_Actions inputActions;
  IInteractable currentInteractableTooltip;
  void Start()
  {
    inputActions = new InputSystem_Actions();
    inputActions.Enable();
    rb = GetComponent<Rigidbody>();

    inputActions.Player.Interact.performed += ActivateInteractables;
  }

  private void ActivateInteractables(UnityEngine.InputSystem.InputAction.CallbackContext obj)
  {
    if (currentInteractableTooltip != null)
    {
      currentInteractableTooltip.Interact(gameObject);
    }
  }

  // Update is called once per frame
  void Update()
  {
    Vector2 moveInput = inputActions.Player.Move.ReadValue<Vector2>();
    Vector3 moveForce = new Vector3(moveInput.x, 0, moveInput.y) * 5f;
    rb.linearVelocity = moveForce;
  }

  void OnTriggerEnter(Collider other)
  {
    IInteractable interactable = other.GetComponent<IInteractable>();
    if (interactable != null)
    {
      UIManager.instance.ShowInteractPrompt(interactable.InteractableName, interactable.InteractableDescription, other.transform.position + Vector3.up * 2f);
      currentInteractableTooltip = interactable;
    }
  }

  void OnTriggerExit(Collider other)
  {
    IInteractable interactable = other.GetComponent<IInteractable>();
    if (interactable == currentInteractableTooltip)
    {
      currentInteractableTooltip = null;
      UIManager.instance.HideInteractPrompt();
    }
  }
}
