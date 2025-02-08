using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class VFXObjectPool : MonoBehaviour
{
    [SerializeField]
    private VfxContainerData vfxContainerData;
    [SerializeField]
    private GameObject[] vfxObjects;
    public Dictionary<int, IObjectPool<DamagedVFX>> vfxDamagedVFXPoolTable { get; private set; }

    private void Awake()
    {

        //foreach (var vfxObject in vfxObjects)
        //{
        //    vfxDamagedVFXPoolTable[0] = new ObjectPool<DamagedVFX>(OnCreateDamagedVFX, OnGetDamagedVFX, OnReleaseDamagedVFX, OnDestroyDamagedVFX, true, 1000);
        //}
        // UiDamageTextPool = new ObjectPool<UIDamageText>(OnCreateDamageText, OnGetDamageText, OnReleaseDamageText, OnDestroyDamageText, true, 1000);
    }

    //private DamagedVFX OnCreateDamagedVFX()
    //{
    //    // Instantiate(vfxContainerData.VfxContainerTable[id][0]).TryGetComponent(out DamagedVFX vfxObject);
    //    // vfxObject.SetPool(vfxDamagedVFXPoolTable[id]);

    //    return vfxObject;
    //}

    private void OnGetDamagedVFX(DamagedVFX vfxObject)
    {
        vfxObject.gameObject.SetActive(true);
    }

    private void OnReleaseDamagedVFX(DamagedVFX vfxObject)
    {
        vfxObject.gameObject.SetActive(false);
    }

    private void OnDestroyDamagedVFX(DamagedVFX vfxObject)
    {
        Destroy(vfxObject.gameObject);
    }
}
