using TMPro;
using UnityEngine;

public class WorldSpaceTooltip : MonoBehaviour
{
  public TMP_Text NameText;
  public TMP_Text DescriptionText;

  public void ShowTooltip(string name, string description, Vector3 position)
  {
    NameText.text = name;
    DescriptionText.text = description;
    transform.position = position;
    gameObject.SetActive(true);
  }

  public void HideTooltip()
  {
    gameObject.SetActive(false);
  }
}