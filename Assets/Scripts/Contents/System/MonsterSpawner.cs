using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class MonsterSpawner : MonoBehaviour, IMonsterSpawner
{
    [SerializeField]
    private MonsterSpawnSystem monsterSpawnSystem;

    [SerializeField]
    private Transform startPoint;
    [SerializeField]
    private Transform[] movePoints;

    private GameObject monsterPrefab;

    public UnityEvent<MonsterFSMController> spawnEvent;
    public UnityAction<MonsterFSMController> deathMonsterAction;
    private Vector2[] moveDirections;

    public GameController GameController { get; private set; }


    protected float spawnTime;
    protected float currentSpawnTime = 0f;
    protected int currentSpawnCount = 0;

    protected bool isActive = false;
    protected bool isRepeat = true;

    private WaveData waveData;
    private MonsterData monsterData;
    private Coroutine spawnCoroutine;

    private void Awake()
    {
        CreateMoveDirection();
    }

    private void Start()
    {
        GameController = monsterSpawnSystem.GameController;
    }

    public virtual void SetMonsterWaveData(WaveData monsterSpawnInfo)
    {
        waveData = monsterSpawnInfo;
        spawnTime = waveData.SpawnInterval;
        monsterData = DataTableManager.MonsterDataTable.Get(waveData.MonsterID);
        monsterPrefab = monsterData.PrefabObject;
    }

    public virtual void StartSpawn()
    {

        spawnCoroutine = StartCoroutine(StartSpawnCoroutine());
        //if (monsterSpawnInfo.IsRepeat)
        //    spawnCoroutine = StartCoroutine(StartSpawnRepeatCoroutine());
        //else
        //    spawnCoroutine = StartCoroutine(StartSpawnCoroutine());

        isActive = true;
        enabled = true;
    }

    public virtual void StartSpawn(bool isRepeat)
    {
        if(isRepeat)
            spawnCoroutine = StartCoroutine(StartSpawnRepeatCoroutine());
        else
            spawnCoroutine = StartCoroutine(StartSpawnCoroutine());

        isActive = true;
        enabled = true;
    }
    public virtual void StopSpawn()
    {
        currentSpawnCount = 0;
        isActive = false;
        //enabled = false;
        if(spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
            spawnCoroutine = null;
        }
    }

    private IEnumerator StartSpawnCoroutine()
    {
        currentSpawnCount = 0; 
        ISpawn();

        while (currentSpawnCount < waveData.SpawnCount)
        {
            currentSpawnTime += Time.deltaTime;

            if(currentSpawnTime >= spawnTime)
            {
                ISpawn();
                currentSpawnTime -= spawnTime;
            }

            yield return null;
        }

        monsterSpawnSystem.EndSpawn();
    }
    private IEnumerator StartSpawnRepeatCoroutine()
    {
        ISpawn();

        while (true)
        {
            currentSpawnTime += Time.deltaTime;

            if (currentSpawnTime >= spawnTime)
            {
                ISpawn();
                currentSpawnTime -= spawnTime;

                //if (monsterSpawnInfo.SpawnCount == currentSpawnCount)
                //{
                //    StopSpawn();
                //}
            }

            yield return null;
        }
    }

    public virtual void ISpawn()
    {
        GameObject monster = Instantiate(monsterPrefab);
        monster.transform.position = startPoint.transform.position;

        var monsterController = monster.GetComponent<MonsterFSMController>();
        monsterController.OnSpawn(this);
        spawnEvent?.Invoke(monsterController);

        var monsterStatus = monster.GetComponent<MonsterStatus>();
        monsterStatus.CurrentValueTable[StatType.MovementSpeed].SetValue(monsterData.MoveSpeed);
        monsterStatus.CurrentValueTable[StatType.HP].SetValue(monsterData.Hp);
        int monsterCoinQty = monsterData.CoinQty;
        int jewelQty = monsterData.JewelQty;

        monsterStatus.DeathEvent.AddListener(() => deathMonsterAction.Invoke(monsterController));
        monsterStatus.DeathEvent.AddListener(() => { if(monsterCoinQty != 0) GameController.OnAddCoin(monsterCoinQty); if(jewelQty != 0) GameController.OnAddJewel(jewelQty); });

        if(monsterData.MonsterType == MonsterType.Boss)
        {
            monsterSpawnSystem.OnAddBossMonster();
            monsterStatus.DeathEvent.AddListener(monsterSpawnSystem.OnDeathBossMonster);
        }

        
        // enemyController.GetComponent<IDamageable>()?.DeathEvent.AddListener(() => deathMonsterAction.Invoke(enemyController));
        // enemyController.SetDestinationPoint(this, movePoints[0].position);

        ++currentSpawnCount;
    }

    public bool GetMovePoint(ref Vector2 movePoint, ref Vector2 moveDirection, ref int index)
    {
        if(index == movePoints.Length)
        {
            index = 0;
        }

        movePoint = movePoints[(index + 1) % movePoints.Length].position;
        moveDirection = moveDirections[index];

        return true;
    }
    // public bool 


    private void CreateMoveDirection()
    {
        int moveCount = movePoints.Length;

        moveDirections = new Vector2[moveCount];

        int startPointIndex = 0;
        for (int i = 0; i < moveCount; ++i)
        {
            if(startPoint == movePoints[i])
            {
                startPointIndex = i;
                break;
            }
        }

        for (int i = 0; i < moveCount; ++i)
        {
            var moveDirection = movePoints[(startPointIndex + 1) % moveCount].position - movePoints[startPointIndex].position;
            moveDirections[i] = moveDirection.normalized;
            ++startPointIndex;
        }
    }

    public void SetMonsterDeathAction(UnityAction<MonsterFSMController> onDeathMonster)
    {
        deathMonsterAction = onDeathMonster;
    }
}
