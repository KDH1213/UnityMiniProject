using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class UIDamageObjectTextPool : MonoBehaviour
{
    [SerializeField]
    private GameObject uIDamageTextPrefab;

    public IObjectPool<UIDamageText> UiDamageTextPool { get; private set; }

    [SerializeField]
    private int defalutCreateCount = 200;

    private void Awake()
    {
        UiDamageTextPool = new ObjectPool<UIDamageText>(OnCreateDamageText, OnGetDamageText, OnReleaseDamageText, OnDestroyDamageText, true, 1000);
        List<UIDamageText> list = new List<UIDamageText>();

        for (int i = 0; i < defalutCreateCount; ++i)
        {
            list.Add(UiDamageTextPool.Get());
        }

        foreach (var @object in list)
        {
            @object.DestroyUIDamageText();
        }
    }

    private UIDamageText OnCreateDamageText()
    {
        Instantiate(uIDamageTextPrefab).TryGetComponent(out UIDamageText damageText);

        damageText.SetPool(UiDamageTextPool);

        return damageText;
    }

    private void OnGetDamageText(UIDamageText uIDamageText)
    {
        uIDamageText.gameObject.SetActive(true);
    }

    private void OnReleaseDamageText(UIDamageText uIDamageText)
    {
        uIDamageText.gameObject.SetActive(false);
    }

    private void OnDestroyDamageText(UIDamageText uIDamageText)
    {
        Destroy(uIDamageText.gameObject);
    }
}
