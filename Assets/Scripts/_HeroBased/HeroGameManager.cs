using UnityEngine;
using UnityEngine.SceneManagement;

public enum HeroLevelType
{
  Combat,
  Shop,
  Encounter,
  Elite,
  Boss,
}

public class HeroGameManager : MonoBehaviour
{
  public static HeroGameManager instance;

  public HeroLevelType selectedLevelType;
  public int selectedLevelTier = 0;

  void Awake()
  {
    if (instance == null)
    {
      instance = this;
      DontDestroyOnLoad(gameObject);
    }
    else
    {
      Destroy(gameObject);
    }
  }


  public void EnterLevel(HeroLevelType hlType, int levelTier)
  {
    switch (hlType)
    {
      case HeroLevelType.Combat:
        selectedLevelType = hlType;
        selectedLevelTier = levelTier;
        SceneManager.LoadScene("Combat");
        break;
    }
  }
}
