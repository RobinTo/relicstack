using System.Collections.Generic;
using UnityEngine;

public class CommanderController : MonoBehaviour
{
  [SerializeField]
  Transform pickupPoint;

  Rigidbody rb;
  IInteractable currentInteractableTooltip;
  IPickupable currentPickupableTooltip;

  IPickupable carrying;

  void Start()
  {
    rb = GetComponent<Rigidbody>();

    InputManager.Instance.Controls.Player.Interact.performed += ActivateInteractables;
    InputManager.Instance.Controls.Player.Pickup.performed += PickupItem;
  }

  private void PickupItem(UnityEngine.InputSystem.InputAction.CallbackContext obj)
  {
    Debug.Log("Pickup action triggered");
    if (carrying == null && currentPickupableTooltip != null)
    {
      GameObject pickupGO = (currentPickupableTooltip as MonoBehaviour).gameObject;
      carrying = currentPickupableTooltip;
      pickupGO.transform.parent = pickupPoint;
      pickupGO.transform.localPosition = Vector3.zero;
      UIManager.instance.HidePickupPrompt();
    }
    else
    {
      DropItem();
    }
  }

  private void DropItem()
  {
    if (carrying != null)
    {
      GameObject pickupGO = (carrying as MonoBehaviour).gameObject;
      carrying = null;
      pickupGO.transform.parent = null;
    }
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
    Vector2 moveInput = InputManager.Instance.Controls.Player.Move.ReadValue<Vector2>();
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

    IPickupable pickupable = other.GetComponent<IPickupable>();
    if (pickupable != null)
    {
      UIManager.instance.ShowPickupPrompt(pickupable.PickupableName, pickupable.PickupableDescription, other.transform.position + Vector3.up * 2f);
      currentPickupableTooltip = pickupable;
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

    IPickupable pickupable = other.GetComponent<IPickupable>();
    if (pickupable == currentPickupableTooltip)
    {
      currentPickupableTooltip = null;
      UIManager.instance.HidePickupPrompt();
    }
  }

  public void HandlePlacerEnter(Collider other)
  {
    if (carrying != null)
    {
      Placer placer = other.GetComponent<Placer>();
      if (placer != null)
      {
        placer.Place(carrying);
        carrying = null;
      }
    }
  }

  public void HandlePlacerExit(Collider other)
  {

  }
}
