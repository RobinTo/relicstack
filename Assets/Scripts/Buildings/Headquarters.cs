using UnityEngine;

public class Headquarters : MonoBehaviour
{
  public static Headquarters instance;

  [SerializeField]
  int health = 20;

  void Awake()
  {
    if (instance == null)
    {
      instance = this;
    }
    else
    {
      Destroy(gameObject);
    }
  }

  void OnTriggerEnter(Collider other)
  {
    Unit unit = other.GetComponent<Unit>();
    if (unit != null && unit.IsEnemy)
    {
      Debug.Log("Base lost health!");
      Destroy(unit.gameObject);
      health--;

      if (health < 0)
      {
        Debug.Log("Base destroyed, you lose!");
        Destroy(gameObject);
      }
    }
  }
}
