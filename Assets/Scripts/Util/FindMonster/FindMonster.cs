using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

public class FindMonster : MonoBehaviour
{
    [SerializeField]
    private MonsterManager monsterManager;

    private List<GameObject> gameObjectList = new List<GameObject>();

    public GameObject FindAttackTarget(Vector3 targetPosition, float attackRange)
    {
        var currentMonsterList = monsterManager.CurrentMonsterList;

        int monsterCount = currentMonsterList.Count;

        if(monsterCount == 0)
        {
            return null;
        }

        NativeList<FindMonsterInfo> findMonsterInfoList = new NativeList<FindMonsterInfo>(monsterCount, Allocator.TempJob);

        SortJob<FindMonsterInfo, FindMonsterInfoComparer> sortJob = findMonsterInfoList.SortJob(new FindMonsterInfoComparer { });
        JobHandle sortHandle = sortJob.Schedule();

        FindMonsterJob findMonsterJob = new FindMonsterJob
        {
            monsterPositions = monsterManager.MonsterPositions,
            monsterRadiuss = monsterManager.MonsterRadiuss,
            targetPostion = targetPosition,
            findDistance = attackRange * attackRange,
            findMonsterInfoList = findMonsterInfoList,
        };

        JobHandle findHandle = findMonsterJob.Schedule(monsterCount, 50);
        findHandle.Complete();

        var targetObject = currentMonsterList[0] != null ? currentMonsterList[0].gameObject : null;

        findMonsterInfoList.Dispose();

        return targetObject;
    }

    public List<GameObject> FindAttackTargets(Vector3 targetPosition, float attackRange)
    {
        gameObjectList.Clear();
        var currentMonsterList = monsterManager.CurrentMonsterList;

        int monsterCount = currentMonsterList.Count;

        if (monsterCount == 0)
        {
            return null;
        }

        NativeList<FindMonsterInfo> findMonsterInfoList = new NativeList<FindMonsterInfo>(monsterCount, Allocator.TempJob);

        SortJob<FindMonsterInfo, FindMonsterInfoComparer> sortJob = findMonsterInfoList.SortJob(new FindMonsterInfoComparer { });
        JobHandle sortHandle = sortJob.Schedule();

        FindMonsterJob findMonsterJob = new FindMonsterJob
        {
            monsterPositions = monsterManager.MonsterPositions,
            monsterRadiuss = monsterManager.MonsterRadiuss,
            targetPostion = targetPosition,
            findDistance = attackRange,
            findMonsterInfoList = findMonsterInfoList,
        };

        JobHandle findHandle = findMonsterJob.Schedule(monsterCount, 50);
        findHandle.Complete();

        foreach (var findMonsterInfo in findMonsterInfoList)
        {
            gameObjectList.Add(currentMonsterList[findMonsterInfo.index].gameObject);
        }

        findMonsterInfoList.Dispose();

        return gameObjectList;
    }
}
