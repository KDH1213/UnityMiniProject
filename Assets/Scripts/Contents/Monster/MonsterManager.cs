using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MonsterManager : MonoBehaviour
{
    [SerializeField]
    private List<MonsterFSMController> currentMonsterList = new List<MonsterFSMController>();

    [SerializeField]
    private List<MonsterFSMController> startDeathMonsterList = new List<MonsterFSMController>();
    private List<MonsterFSMController> deathMonsterList = new List<MonsterFSMController>();
    private List<MonsterFSMController> destroyMonsterList = new List<MonsterFSMController>();

    public UnityEvent<int> changeMonsterCount;
    public int CurrentMonsterCount { get { return currentMonsterList.Count; } }
    
    private bool isDeathMonster = false;
    private bool isDestroyMonster = false;
    private void Awake()
    {
        currentMonsterList.Capacity = 80;
        startDeathMonsterList.Capacity = 20;
        deathMonsterList.Capacity = 40;
        destroyMonsterList.Capacity = 20;
    }

    private void Start()
    {
        changeMonsterCount?.Invoke(0);
    }

    public void OnAddMonster(MonsterFSMController monsterFSMController)
    {
        //if (currentMonsterList.Contains(monsterFSMController))
        //    return;
        
        currentMonsterList.Add(monsterFSMController);
        changeMonsterCount?.Invoke(CurrentMonsterCount);
    }

    public void OnDeathMonster(MonsterFSMController monsterFSMController)
    {
        //if (!currentMonsterList.Contains(monsterFSMController))
        //    return;

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
        foreach (var monster in currentMonsterList)
        {
            monster.ExcuteUpdate();
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
