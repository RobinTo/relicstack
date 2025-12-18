using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
  public static InputManager Instance;

  InputSystem_Actions inputActions;
  public InputSystem_Actions Controls => inputActions;

  void Awake()
  {
    if (Instance == null)
    {
      Instance = this;
      DontDestroyOnLoad(gameObject);
    }
    else
    {
      Destroy(gameObject);
    }
    inputActions = new InputSystem_Actions();
    inputActions.Enable();
    SetGamePlayActionMapActive();
  }

  public void SetGamePlayActionMapActive()
  {
    inputActions.Player.Enable();
    inputActions.UI.Disable();
  }

  public void SetUIActionMapActive()
  {
    inputActions.Player.Disable();
    inputActions.UI.Enable();
  }
}
