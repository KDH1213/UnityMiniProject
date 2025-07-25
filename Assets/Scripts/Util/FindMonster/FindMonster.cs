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

    public List<GameObject> FindAttackTarget(Vector3 targetPosition, float attackRange)
    {
        gameObjectList.Clear();
        var currentMonsterList = monsterManager.CurrentMonsterList;

        int monsterCount = currentMonsterList.Count;

        NativeArray<float3> monsterPositions = new NativeArray<float3>(monsterCount, Allocator.Persistent);
        NativeArray<float> monsterRadiuss = new NativeArray<float>(monsterCount, Allocator.Persistent);
        NativeList<FindMonsterInfo> findMonsterInfoList = new NativeList<FindMonsterInfo>();


        for (int i = 0; i < monsterCount; ++i)
        {
            monsterPositions[i] = currentMonsterList[i].transform.localPosition;
            monsterRadiuss[i] = currentMonsterList[i].transform.localScale.x;
        }

        // SortJob<float3, AxisXComparer> sortJob = TargetPositions.SortJob(new AxisXComparer { });
        // JobHandle sortHandle = sortJob.Schedule();

        FindMonsterJob findMonsterJob = new FindMonsterJob
        {
            monsterPositions = monsterPositions,
            monsterRadiuss = monsterRadiuss,
            targetPostion = targetPosition,
            findDistance = attackRange,
            findMonsterInfoList = findMonsterInfoList,
        };

        JobHandle findHandle = findMonsterJob.Schedule(monsterCount, 100);
        findHandle.Complete();

        foreach (var findMonsterInfo in findMonsterInfoList)
        {
            gameObjectList.Add(currentMonsterList[findMonsterInfo.index].gameObject);
        }


        monsterPositions.Dispose();
        monsterRadiuss.Dispose();
        findMonsterInfoList.Dispose();

        return gameObjectList;
    }
}
