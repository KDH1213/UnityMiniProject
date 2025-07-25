using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;

public class MonsterManager : MonoBehaviour
{
    private List<MonsterFSMController> currentMonsterList = new List<MonsterFSMController>();
    private List<MonsterFSMController> startDeathMonsterList = new List<MonsterFSMController>();

    public List<MonsterFSMController> CurrentMonsterList { get { return currentMonsterList; } }
    private List<MonsterFSMController> deathMonsterList = new List<MonsterFSMController>();
    private List<MonsterFSMController> destroyMonsterList = new List<MonsterFSMController>();

    public UnityEvent<int> changeMonsterCount;
    public int CurrentMonsterCount { get { return currentMonsterList.Count; } }
    
    private bool isDeathMonster = false;
    private bool isDestroyMonster = false;

    [SerializeField]
    private bool isJob = true;

    private NativeList<float3> monsterPositions;
    public NativeList<float3> MonsterPositions { get { return monsterPositions; } }

    private NativeList<float> monsterRadiuss;
    public NativeList<float> MonsterRadiuss { get { return monsterRadiuss; } }

    private void Awake()
    {
        currentMonsterList.Capacity = 1000;
        startDeathMonsterList.Capacity = 20;
        deathMonsterList.Capacity = 40;
        destroyMonsterList.Capacity = 20;
    }

    private void Start()
    {
        changeMonsterCount?.Invoke(0);

        if(isJob)
        {
            monsterPositions = new NativeList<float3>(10000, Allocator.Persistent);
            monsterRadiuss = new NativeList<float>(10000, Allocator.Persistent);
        }
    }

    private void OnDestroy()
    {
        if (isJob)
        {
            monsterPositions.Dispose();
            monsterRadiuss.Dispose();
        }
    }

    public void OnAddMonster(MonsterFSMController monsterFSMController)
    {
        if (currentMonsterList.Contains(monsterFSMController))
            return;

        currentMonsterList.Add(monsterFSMController);
        changeMonsterCount?.Invoke(CurrentMonsterCount);
    }

    public void OnDeathMonster(MonsterFSMController monsterFSMController)
    {
        if (!currentMonsterList.Contains(monsterFSMController))
            return;

        startDeathMonsterList.Add(monsterFSMController);
        isDeathMonster = true;
    }

    private void AddDeathMonster()
    {
        foreach(var monster in startDeathMonsterList)
        {
            if (currentMonsterList.Contains(monster))
            {
                currentMonsterList.Remove(monster);
            }
            if (!deathMonsterList.Contains(monster))
            {
                deathMonsterList.Add(monster);
            }
        }

        changeMonsterCount?.Invoke(CurrentMonsterCount);
        startDeathMonsterList.Clear();
        isDeathMonster= false;
    }

    private void RemoveDestroyMonster()
    {
        foreach (var monster in destroyMonsterList)
        {
            if (deathMonsterList.Contains(monster))
                deathMonsterList.Remove(monster);
        }

        destroyMonsterList.Clear();
    }

    public void OnDestroyMonster(MonsterFSMController monsterFSMController)
    {
        destroyMonsterList.Add(monsterFSMController);
        isDestroyMonster = true;
    }

    private void Update()
    {
        if(isJob)
        {
            monsterPositions.Clear();
            monsterRadiuss.Clear();

            foreach (var monster in currentMonsterList)
            {
                monster.ExcuteUpdate();

                if(monster.CurrentStateType != MonsterStateType.Death)
                {
                    monsterPositions.Add(monster.transform.position);
                    monsterRadiuss.Add(2f);
                }               
            }
        }
        else
        {
            foreach (var monster in currentMonsterList)
            {
                monster.ExcuteUpdate();
            }
        }

        foreach (var monster in deathMonsterList)
        {
            monster.ExcuteUpdate();
        }

        if(isDeathMonster)
        {
            AddDeathMonster();
        }

        if(isDestroyMonster)
        {
            RemoveDestroyMonster();
        }
    }

    //private void FixedUpdate()
    //{
    //    foreach (var monster in currentMonsterList)
    //    {
    //        monster.ExcuteFixedUpdate();
    //    }
    //}
}
