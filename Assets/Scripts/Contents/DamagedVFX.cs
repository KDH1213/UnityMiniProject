using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class DamagedVFX : MonoBehaviour
{
    private IObjectPool<DamagedVFX> damagedVFXPool;

    private void OnDisable()
    {
        damagedVFXPool.Release(this);
    }

    public void SetPool(IObjectPool<DamagedVFX> damagedVFXPool)
    {
        this.damagedVFXPool = damagedVFXPool;
    }

    public IObjectPool<DamagedVFX> GetObjectPool()
    {
        return damagedVFXPool;
    }
}
