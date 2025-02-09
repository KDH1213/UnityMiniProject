using UnityEngine;
using UnityEngine.Pool;

public class AddCurrencyEffectPool : MonoBehaviour
{
    [SerializeField]
    private GameObject addCurrencyEffectPrefab;

    public IObjectPool<AddCurrencyEffect> AddCurrencyEffectObjectPool { get; private set; }

    private void Awake()
    {
        AddCurrencyEffectObjectPool = new ObjectPool<AddCurrencyEffect>(OnCreateAddCurrencyEffect, OnGetAddCurrencyEffect, OnReleaseAddCurrencyEffect, OnDestroyAddCurrencyEffect, true, 100);
    }

    private AddCurrencyEffect OnCreateAddCurrencyEffect()
    {
        Instantiate(addCurrencyEffectPrefab).TryGetComponent(out AddCurrencyEffect addCurrencyEffect);

        addCurrencyEffect.SetPool(AddCurrencyEffectObjectPool);

        return addCurrencyEffect;
    }

    private void OnGetAddCurrencyEffect(AddCurrencyEffect addCurrencyEffect)
    {
        addCurrencyEffect.gameObject.SetActive(true);
    }

    private void OnReleaseAddCurrencyEffect(AddCurrencyEffect addCurrencyEffect)
    {
        addCurrencyEffect.gameObject.SetActive(false);
    }

    private void OnDestroyAddCurrencyEffect(AddCurrencyEffect addCurrencyEffect)
    {
        Destroy(addCurrencyEffect.gameObject);
    }
}
