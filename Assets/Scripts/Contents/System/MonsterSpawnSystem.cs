using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MonsterSpawnSystem : MonoBehaviour
{
    [SerializeField]
    private MonsterManager monsterManager;
    [field: SerializeField]
    public GameController GameController { get; private set; }

    [SerializeField] 
    private List<MonsterSpawner> monsterSpawnerList;
    private List<WaveData> waveDataList = new List<WaveData>();

    [SerializeField] 
    private bool useAutoStart = false;

    public UnityEvent<int, int> changeWaveEvent;
    public UnityEvent<float> changeWaveTimeEvent;
    public UnityEvent bossWaveEvnet;

    private int activeSpawnerCount;
    private int currentWaveLevel = 0;
    public int CurrentWaveLevel { get  { return currentWaveLevel; } }

    private int bossMonsterCount = 0;
    private bool isActive = false;

    private Coroutine coSpawn;
    private UniTask uniTaskSpawn;

    private void Awake()
    {
        waveDataList = DataTableManager.WaveDataTable.List;
    }

    private void Start()
    {
        changeWaveEvent?.Invoke(currentWaveLevel, waveDataList.Count);

        foreach (var spawner in monsterSpawnerList)
        {
            spawner.spawnEvent.AddListener(monsterManager.OnAddMonster);
            spawner.SetMonsterDeathAction(monsterManager.OnDeathMonster);
            spawner.SetMonsterDestroyAction(monsterManager.OnDestroyMonster);
        }

        coSpawn = StartCoroutine(CoStartSpawn());
        // uniTaskSpawn = UniTaskStartSpawn();

        //if (useAutoStart)
        //{
        //    StartSpawn();
        //}
    }


    public void StartSpawn()
    {
        isActive = true;
        activeSpawnerCount = 0;
        foreach (var spawner in monsterSpawnerList)
        {
            spawner.SetMonsterWaveData(waveDataList[currentWaveLevel]);
            spawner.StartSpawn();
            ++activeSpawnerCount; 
        }
        changeWaveEvent?.Invoke(currentWaveLevel + 1, waveDataList.Count);
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
        float currentTime = 0f;
        bool isGameOver = false;
        while (currentWaveLevel < maxWave)
        {
            currentTime = waveDataList[currentWaveLevel].SpawnWaitTime;
            while (currentTime > 0f)
            {
                yield return new WaitForEndOfFrame();
                currentTime -= Time.deltaTime;
                changeWaveTimeEvent?.Invoke(currentTime);
            }

            if(bossMonsterCount != 0)
            {
                GameController.GameOver();
                isGameOver = true;
                break;
            }

            GameController.AddCurrencyType(waveDataList[currentWaveLevel].CurrencyType, waveDataList[currentWaveLevel].WaveStartCurrencyValue);
            StartSpawn();

            currentTime += waveDataList[currentWaveLevel++].SpawnTime;
            while (currentTime > 0f)
            {
                yield return new WaitForEndOfFrame();
                currentTime -= Time.deltaTime;
                changeWaveTimeEvent?.Invoke(currentTime);
            }
        }

        if(!isGameOver)
            GameController.GameClear();
    }

    private async UniTask UniTaskStartSpawn()
    {
        int maxWave = waveDataList.Count;
        float currentTime = 0f;
        bool isGameOver = false;
        while (currentWaveLevel < maxWave)
        {
            currentTime = waveDataList[currentWaveLevel].SpawnWaitTime;
            while (currentTime > 0f)
            {
                await UniTask.Yield(PlayerLoopTiming.Update);
                currentTime -= Time.deltaTime;
                changeWaveTimeEvent?.Invoke(currentTime);

            }

            if (bossMonsterCount != 0)
            {
                GameController.GameOver();
                isGameOver = true;
                break;
            }


            GameController.AddCurrencyType(waveDataList[currentWaveLevel].CurrencyType, waveDataList[currentWaveLevel].WaveStartCurrencyValue);
            StartSpawn();

            currentTime += waveDataList[currentWaveLevel++].SpawnTime;
            while (currentTime > 0f)
            {
                await UniTask.Yield(PlayerLoopTiming.Update);
                currentTime -= Time.deltaTime;
                changeWaveTimeEvent?.Invoke(currentTime);
            }
        }

        if (!isGameOver)
            GameController.GameClear();
    }

    public void OnDeathBossMonster()
    {
        --bossMonsterCount;

        if(bossMonsterCount < 0)
            bossMonsterCount = 0;
    }

    public void OnAddBossMonster()
    {
        ++bossMonsterCount;
    }
}
