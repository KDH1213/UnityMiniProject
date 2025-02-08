using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class MonsterObjectPool : MonoBehaviour
{
    public int CreateMonsterID { get; set; }
    public Dictionary<int, IObjectPool<MonsterFSMController>> monsterPoolTable { get; private set; } = new Dictionary<int, IObjectPool<MonsterFSMController>>();

    private GameObject monsterObject;

    private MonsterFSMController OnCreateMonster()
    {
        Instantiate(monsterObject).TryGetComponent(out MonsterFSMController monster);
        monster.SetPool(monsterPoolTable[CreateMonsterID]);
        return monster;
    }

    private void OnGetMonster(MonsterFSMController monster)
    {
        monster.gameObject.SetActive(true);
    }

    private void OnReleaseMonster(MonsterFSMController monster)
    {
        monster.gameObject.SetActive(false);
    }

    private void OnDestroyMonster(MonsterFSMController monster)
    {
        Destroy(monster.gameObject);
    }

    public MonsterFSMController GetMonster()
    {
        return monsterPoolTable[CreateMonsterID].Get();
    }

    public void SetMonsterData(GameObject prefabObject, int id)
    {
        monsterObject = prefabObject;
        CreateMonsterID = id;

        if (!monsterPoolTable.ContainsKey(CreateMonsterID))
        {
            monsterPoolTable.Add(CreateMonsterID, new ObjectPool<MonsterFSMController>(OnCreateMonster, OnGetMonster, OnReleaseMonster, OnDestroyMonster, true, 100));
        }
    }
}
