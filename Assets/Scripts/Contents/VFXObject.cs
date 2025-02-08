using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class VFXObject : MonoBehaviour
{
    [field: SerializeField]
    public int ID { get; private set; }
    private IObjectPool<VFXObject> vFXPool;

    private void OnDisable()
    {
        vFXPool.Release(this);
    }

    public void SetPool(IObjectPool<VFXObject> VFXPool)
    {
        this.vFXPool = VFXPool;
    }

    public IObjectPool<VFXObject> GetObjectPool()
    {
        return vFXPool;
    }
}
