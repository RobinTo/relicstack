using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class BuildingArea : MonoBehaviour, IInteractable
{
  [SerializeField]
  GameObject builds;

  public string InteractableName => "Unit Producer";
  public string InteractableDescription => "Produces units at regular intervals.";

  bool spent = false;

  public void Interact(GameObject interactor)
  {
    if (!spent && Player.instance.Gold > 5)
    {
      spent = true;
      Player.instance.SpendGold(5);
      StartCoroutine(BuildBuilding());
    }
  }

  IEnumerator BuildBuilding()
  {
    GameObject go = Instantiate(builds, transform.position + Vector3.up * 5, transform.rotation);
    float startingY = go.transform.position.y;
    float timer = 0;
    while (timer < 1)
    {
      timer += Time.deltaTime;
      go.transform.position = new Vector3(go.transform.position.x, Mathf.Lerp(startingY, 0, timer * timer), go.transform.position.z);
      yield return new WaitForEndOfFrame();
    }
    go.transform.position = new Vector3(go.transform.position.x, 0, go.transform.position.z);
    Destroy(gameObject);
  }
}
