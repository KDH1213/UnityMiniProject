using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class UIDamageObjectTextPool : MonoBehaviour
{
    [SerializeField]
    private GameObject uIDamageTextPrefab;
    [SerializeField]
    private GameObject uIDamageTextMeshRendererPrefab;

    public IObjectPool<UIDamageText> UiDamageTextPool { get; private set; }

    public IObjectPool<UIDamageTextMeshRenderer> UiDamageTextMeshRendererPool { get; private set; }

    [SerializeField]
    private int defalutCreateCount = 200;

    private void Awake()
    {
        //UiDamageTextPool = new ObjectPool<UIDamageText>(OnCreateDamageText, OnGetDamageText, OnReleaseDamageText, OnDestroyDamageText, true, 1000);
        //List<UIDamageText> list = new List<UIDamageText>();

        UiDamageTextMeshRendererPool = new ObjectPool<UIDamageTextMeshRenderer>(OnCreateDamageTextMeshRenderer, OnGetDamageTextMeshRenderer, OnReleaseDamageTextMeshRenderer, OnDestroyDamageTextMeshRenderer, true, 1000);
        List<UIDamageTextMeshRenderer> listMeshRenderer = new List<UIDamageTextMeshRenderer>();

        //for (int i = 0; i < defalutCreateCount; ++i)
        //{
        //    list.Add(UiDamageTextPool.Get());
        //}

        for (int i = 0; i < defalutCreateCount; ++i)
        {
            listMeshRenderer.Add(UiDamageTextMeshRendererPool.Get());
        }

        //foreach (var @object in list)
        //{
        //    @object.DestroyUIDamageText();
        //}

        foreach (var @object in listMeshRenderer)
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

    private void OnGetDamageTextMeshRenderer(UIDamageTextMeshRenderer uIDamageText)
    {
        uIDamageText.gameObject.SetActive(true);
    }

    private void OnReleaseDamageTextMeshRenderer(UIDamageTextMeshRenderer uIDamageText)
    {
        uIDamageText.gameObject.SetActive(false);
    }

    private void OnDestroyDamageTextMeshRenderer(UIDamageTextMeshRenderer uIDamageText)
    {
        Destroy(uIDamageText.gameObject);
    }

    private UIDamageTextMeshRenderer OnCreateDamageTextMeshRenderer()
    {
        Instantiate(uIDamageTextMeshRendererPrefab).TryGetComponent(out UIDamageTextMeshRenderer damageText);

        damageText.SetPool(UiDamageTextMeshRendererPool);

        return damageText;
    }
}
