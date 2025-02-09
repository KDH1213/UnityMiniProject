using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class DamagedObjectPool : MonoBehaviour
{
    private GameObject damagedObjectPoolPrefab;

    public AttackType AttackType { get; set; }
    public Dictionary<AttackType, GameObject> damagedObjectPrefabTable = new Dictionary<AttackType, GameObject>();
    public Dictionary<AttackType, IObjectPool<DamagedObject>> DamagedObjectPoolTable { get; private set; } = new Dictionary<AttackType, IObjectPool<DamagedObject>>();

    private void Awake()
    {
        ObjectPoolManager.Instance.ObjectPoolTable.Add(Tags.DamagedObjectPool, this);

        var attackTable = DataTableManager.AttackDataTable.AttackTable;

        foreach (var attackData in attackTable)
        {
            SetDamagedID(attackData.Value.PrefabObject, attackData.Value.AttackType);
        }
    }

    private DamagedObject OnCreateDamagedObject()
    {
        Instantiate(damagedObjectPrefabTable[AttackType]).TryGetComponent(out DamagedObject DamagedObject);

        DamagedObject.SetPool(DamagedObjectPoolTable[AttackType]);

        return DamagedObject;
    }

    private void OnGetAddDamagedObject(DamagedObject DamagedObject)
    {
        DamagedObject.gameObject.SetActive(true);
    }

    private void OnReleaseDamagedObject(DamagedObject DamagedObject)
    {
        DamagedObject.gameObject.SetActive(false);
    }

    private void OnDestroyDamagedObject(DamagedObject DamagedObject)
    {
        Destroy(DamagedObject.gameObject);
    }
    public DamagedObject GetDamagedObject(AttackType attackType)
    {
        AttackType = attackType;
        return DamagedObjectPoolTable[attackType].Get();
    }

    public void SetDamagedID(GameObject prefabObject, AttackType attackType)
    {
        AttackType = attackType;

        if (!DamagedObjectPoolTable.ContainsKey(AttackType))
        {
            damagedObjectPrefabTable.Add(AttackType, prefabObject);
            DamagedObjectPoolTable.Add(AttackType, new ObjectPool<DamagedObject>(OnCreateDamagedObject, OnGetAddDamagedObject, OnReleaseDamagedObject, OnDestroyDamagedObject, true, 100));
        }
    }
}
