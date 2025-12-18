using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour, IInteractable
{

  public string InteractableName => "Shop";
  public string InteractableDescription => "A place to buy items.";

  [SerializeField]
  List<RelicSO> itemsForSale = new List<RelicSO>();

  public void Interact(GameObject interactor)
  {
    Debug.Log("Interacted with Shop");
    OpenShop();
  }

  void OpenShop()
  {
    List<ChoiceData> choices = new List<ChoiceData>();
    List<RelicSO> itemsForSaleCopy = new List<RelicSO>(itemsForSale);
    for (int i = 0; i < 3; i++)
    {
      int idx = Random.Range(0, itemsForSaleCopy.Count);
      RelicSO item = itemsForSaleCopy[idx];
      choices.Add(new ChoiceData(item.relicName, () =>
      {
        Instantiate(item.prefab, transform.position - Vector3.forward + Vector3.up, Quaternion.identity);
        CloseShop();
      }));
      itemsForSaleCopy.RemoveAt(idx);
    }

    InputManager.Instance.SetUIActionMapActive();
    UIManager.instance.ShowChoicePanel(choices);
    InputManager.Instance.Controls.UI.Cancel.performed += OnCancel;
    Time.timeScale = 0f;
  }

  private void OnCancel(UnityEngine.InputSystem.InputAction.CallbackContext obj)
  {
    CloseShop();
  }

  void CloseShop()
  {
    UIManager.instance.HideChoicePanel();
    InputManager.Instance.SetGamePlayActionMapActive();
    InputManager.Instance.Controls.UI.Cancel.performed -= OnCancel;
    Time.timeScale = 1f;
  }
}