using System.Collections;
using UnityEngine;

public class TestSpell : MonoBehaviour, ISpellInstanceWithOwner
{
  public HeroUnit Owner { get; set; }
  public HeroUnit TargetAtCastTime { get; set; }
  public SpawnGameObjectSpell OwningSpell { get; set; }


  public void OnStart()
  {
    if (TargetAtCastTime != null)
    {
      transform.position = TargetAtCastTime.transform.position;
      StartCoroutine(ReleaseAfter(.5f));
    }
  }

  private IEnumerator ReleaseAfter(float time)
  {
    yield return new WaitForSeconds(time);
    Debug.Log("Releasing", gameObject);
    OwningSpell.ReleaseSpell(this);
  }
}