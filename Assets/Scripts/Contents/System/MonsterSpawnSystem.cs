using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawnSystem : MonoBehaviour
{
    [SerializeField] public List<MonsterSpawner> monsterSpawnerList;

    private List<WaveData> waveDataList = new List<WaveData>();

    [SerializeField] public List<MonsterSpawnInfo> spawnDataByLevelList;
    [SerializeField] private MonsterManager monsterManager;
    [field: SerializeField] public GameController GameController { get; private set; }

    [SerializeField] private bool useAutoStart = false;

    private int currentWaveLevel = 0;
    private bool isActive = false;

    private int activeSpawnerCount;

    private Coroutine coSpawn;

    private void Awake()
    {
        waveDataList = DataTableManager.WaveDataTable.List;
    }

    private void Start()
    {
        // GameController.Instance.SetCurrentWave(currentWaveLevel);

        foreach (var spawner in monsterSpawnerList)
        {
            spawner.spawnEvent.AddListener(monsterManager.OnAddMonster);
            spawner.SetMonsterDeathAction(monsterManager.OnDeathMonster);
        }

        coSpawn = StartCoroutine(CoStartSpawn());

        //if (useAutoStart)
        //{
        //    StartSpawn();
        //}
    }

    public void StartSpawn()
    {
        if (isActive)
            return;

        isActive = true;
        activeSpawnerCount = 0;
        foreach (var spawner in monsterSpawnerList)
        {
            spawner.SetMonsterWaveData(waveDataList[currentWaveLevel]);
            spawner.StartSpawn();
            ++activeSpawnerCount; 
        }
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

    private IEnumerator CoStartSpawn()
    {
        int maxWave = waveDataList.Count;
        while (currentWaveLevel < maxWave)
        {
            yield return new WaitForSeconds(waveDataList[currentWaveLevel].SpawnWaitTime);
            StartSpawn();
            yield return new WaitForSeconds(waveDataList[currentWaveLevel++].SpawnTime);
        }
    }
}
