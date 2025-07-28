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

        NativeQueue<FindMonsterInfo> findMonsterInfoQueue = new NativeQueue<FindMonsterInfo>(Allocator.TempJob);

        SortJob<FindMonsterInfo, FindMonsterInfoComparer> sortJob = findMonsterInfoList.SortJob(new FindMonsterInfoComparer { });
        JobHandle sortHandle = sortJob.Schedule();

        int findIndex = -1;

        if (monsterCount > 100)
        {
            FindMonsterJobParallelFor findMonsterJob = new FindMonsterJobParallelFor
            {
                monsterPositions = monsterManager.MonsterPositions,
                monsterRadiuss = monsterManager.MonsterRadiuss,
                targetPostion = targetPosition,
                findDistance = attackRange * attackRange,
                findMonsterInfoQueue = findMonsterInfoQueue.AsParallelWriter()
            };

            JobHandle findHandle = findMonsterJob.Schedule(monsterCount, 50);
            findHandle.Complete();

            if(findMonsterInfoQueue.TryDequeue(out var findMonsterInfo))
            {
                findIndex = findMonsterInfo.index;
            }
        }
        else
        {
            FindMonsterJob findMonsterJob = new FindMonsterJob
            {
                monsterPositions = monsterManager.MonsterPositions,
                monsterRadiuss = monsterManager.MonsterRadiuss,
                targetPostion = targetPosition,
                findDistance = attackRange * attackRange,
                findMonsterInfoList = findMonsterInfoList,
            };

            JobHandle findHandle = findMonsterJob.Schedule();
            findHandle.Complete();

            if (!findMonsterJob.findMonsterInfoList.IsEmpty)
            {
                findIndex = findMonsterJob.findMonsterInfoList[0].index;
            }
        }

        findMonsterInfoQueue.Dispose();
        findMonsterInfoList.Dispose();

        if(findIndex == -1)
        {
            return null;
        }
        else
        {
            return currentMonsterList[findIndex] != null ? currentMonsterList[findIndex].gameObject : null;
        }
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
        NativeQueue<FindMonsterInfo> findMonsterInfoQueue = new NativeQueue<FindMonsterInfo>(Allocator.TempJob);

        SortJob<FindMonsterInfo, FindMonsterInfoComparer> sortJob = findMonsterInfoList.SortJob(new FindMonsterInfoComparer { });
        JobHandle sortHandle = sortJob.Schedule();

        if (monsterCount > 100)
        {
            FindMonsterJobParallelFor findMonsterJob = new FindMonsterJobParallelFor
            {
                monsterPositions = monsterManager.MonsterPositions,
                monsterRadiuss = monsterManager.MonsterRadiuss,
                targetPostion = targetPosition,
                findDistance = attackRange * attackRange,
                findMonsterInfoQueue = findMonsterInfoQueue.AsParallelWriter(),
            };

            JobHandle findHandle = findMonsterJob.Schedule(monsterCount, 50);
            findHandle.Complete();

        }
        else
        {
            FindMonsterJob findMonsterJob = new FindMonsterJob
            {
                monsterPositions = monsterManager.MonsterPositions,
                monsterRadiuss = monsterManager.MonsterRadiuss,
                targetPostion = targetPosition,
                findDistance = attackRange * attackRange,
                findMonsterInfoList = findMonsterInfoList,
            };

            JobHandle findHandle = findMonsterJob.Schedule();
            findHandle.Complete();
        }

        foreach (var findMonsterInfo in findMonsterInfoList)
        {
            gameObjectList.Add(currentMonsterList[findMonsterInfo.index].gameObject);
        }

        while (findMonsterInfoQueue.TryDequeue(out var result))
        {
            gameObjectList.Add(currentMonsterList[result.index].gameObject);
        }

        findMonsterInfoQueue.Dispose();
        findMonsterInfoList.Dispose();

        return gameObjectList;
    }
}
