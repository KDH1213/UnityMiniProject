using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawnSystem : MonoBehaviour
{
    [SerializeField] public List<MonsterSpawner> monsterSpawnerList;

    [SerializeField] public List<MonsterSpawnInfo> spawnDataByLevelList;
    [SerializeField] private MonsterManager monsterManager;

    [SerializeField] private bool useAutoStart = false;

    private int currentWaveLevel = 0;
    private bool isActive = false;


    private int activeSpawnerCount;

    private void Start()
    {
        // GameController.Instance.SetCurrentWave(currentWaveLevel);

        foreach (var spawner in monsterSpawnerList)
        {
            spawner.spawnEvent.AddListener(monsterManager.OnAddMonster);
            spawner.SetMonsterDeathAction(monsterManager.OnDeathMonster);
        }

        if (useAutoStart)
        {
            StartSpawn();
        }
    }

    public void StartSpawn()
    {
        if (isActive)
            return;

        isActive = true;
        activeSpawnerCount = 0;
        foreach (var spawner in monsterSpawnerList)
        {
            spawner.SetMonsterSpawnInfo(spawnDataByLevelList[currentWaveLevel]);
            spawner.StartSpawn();
            ++activeSpawnerCount;
        }

        currentWaveLevel = Unity.Mathematics.math.min(currentWaveLevel + 1, spawnDataByLevelList.Count - 1);
        // GameController.Instance.SetCurrentWave(currentWaveLevel);
    }

    public void StopSpawn()
    {
        foreach (var spawner in monsterSpawnerList)
        {
            spawner.StopSpawn();
        }
    }

    public void EndSpawn()
    {
        --activeSpawnerCount;

        if(activeSpawnerCount == 0)
        {
            isActive = false;
        }
    }
}
