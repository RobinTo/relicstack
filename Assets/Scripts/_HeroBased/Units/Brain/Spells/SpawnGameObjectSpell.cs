using UnityEngine;
using UnityEngine.Pool;

public interface ISpellInstanceWithOwner
{
  public HeroUnit Owner { get; set; }
  public HeroUnit TargetAtCastTime { get; set; }
  public SpawnGameObjectSpell OwningSpell { get; set; }

  public void OnStart();
}
public abstract class Spell : MonoBehaviour
{
  public abstract void CastSpell();
}

public class SpawnGameObjectSpell : Spell
{
  [SerializeField]
  HeroUnit owner;

  [SerializeField]
  GameObject prefab;

  private ObjectPool<ISpellInstanceWithOwner> spellPool;

  private void Awake()
  {
    spellPool = new ObjectPool<ISpellInstanceWithOwner>(
      createFunc: CreatePooledItem,
      actionOnGet: OnGetFromPool,
      actionOnRelease: OnReleaseToPool,
      actionOnDestroy: OnDestroyPoolObject,
      collectionCheck: true,
      defaultCapacity: 3,
      maxSize: 20
    );
  }

  private ISpellInstanceWithOwner CreatePooledItem()
  {
    GameObject newPrefab = Instantiate(prefab);
    newPrefab.SetActive(false);

    ISpellInstanceWithOwner spellInstance = newPrefab.GetComponent<ISpellInstanceWithOwner>();
    spellInstance.OwningSpell = this;
    return spellInstance;
  }

  private void OnGetFromPool(ISpellInstanceWithOwner spellInstance)
  {
    GameObject go = (spellInstance as MonoBehaviour).gameObject;
    go.SetActive(true);
  }

  private void OnReleaseToPool(ISpellInstanceWithOwner spellInstance)
  {
    GameObject go = (spellInstance as MonoBehaviour).gameObject;
    go.SetActive(false);
  }

  private void OnDestroyPoolObject(ISpellInstanceWithOwner spellInstance)
  {
    GameObject go = (spellInstance as MonoBehaviour).gameObject;
    Destroy(go);
  }

  public override void CastSpell()
  {
    ISpellInstanceWithOwner spellInstance = spellPool.Get();
    GameObject go = (spellInstance as MonoBehaviour).gameObject;
    go.transform.position = transform.position;
    go.transform.rotation = Quaternion.identity;
    spellInstance.Owner = owner;
    spellInstance.TargetAtCastTime = owner.CurrentTarget;
    spellInstance.OnStart();
  }

  public void ReleaseSpell(ISpellInstanceWithOwner spellInstance)
  {
    spellPool.Release(spellInstance);
  }

  private void OnDestroy()
  {
    spellPool?.Dispose();
  }
}