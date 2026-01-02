using UnityEngine;

public class HeroInteractableLevel : MonoBehaviour, IInteractable
{

  public string InteractableName => "Level";
  public string InteractableDescription => "Select this level";

  [SerializeField]
  private HeroLevelType hlType;
  [SerializeField]
  private int levelTier;

  public void Interact(GameObject interactor)
  {
    HeroGameManager.instance.EnterLevel(hlType, levelTier);
  }
}