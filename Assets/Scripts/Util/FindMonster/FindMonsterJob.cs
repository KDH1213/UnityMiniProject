using System;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

public partial struct FindMonsterInfo
{
    public float distance;
    public int index;

    public FindMonsterInfo(float distance, int index)
    {
        this.distance = distance;
        this.index = index;
    }
}

public struct FindMonsterInfoComparer : IComparer<FindMonsterInfo>
{
    public int Compare(FindMonsterInfo lhs, FindMonsterInfo rhs)
    {
        return lhs.distance.CompareTo(rhs.distance);
    }
}

[BurstCompile]
public partial struct FindMonsterJobParallelFor : IJobParallelFor
{
    [ReadOnly]
    public NativeList<float3> monsterPositions;
    [ReadOnly]
    public NativeList<float> monsterRadiuss;

    [ReadOnly]
    public float3 targetPostion;

    [ReadOnly]
    public float findDistance;

    public NativeQueue<FindMonsterInfo>.ParallelWriter findMonsterInfoQueue;

    // public NativeList<FindMonsterInfo>.ParallelWriter findMonsterInfoList;
    public void Execute(int index)
    {
        if(index >= monsterPositions.Length)
        {
            return;
        }

        var distance = math.distancesq(targetPostion, monsterPositions[index]);
        var targetDistance = findDistance - monsterRadiuss[index];

        if (distance < targetDistance)
        {
            findMonsterInfoQueue.Enqueue(new FindMonsterInfo(distance, index));
            // findMonsterInfoList.AddNoResize(new FindMonsterInfo(distance, index));
        }
    }
}

[BurstCompile]
public partial struct FindMonsterJob : IJob
{
    [ReadOnly]
    public NativeList<float3> monsterPositions;
    [ReadOnly]
    public NativeList<float> monsterRadiuss;

    [ReadOnly]
    public float3 targetPostion;

    [ReadOnly]
    public float findDistance;

    public NativeList<FindMonsterInfo> findMonsterInfoList;
    public void Execute()
    {
        int lenght = monsterPositions.Length;

        for (int i = 0; i < lenght; ++i)
        {
            var distance = math.distancesq(targetPostion, monsterPositions[i]);
            var targetDistance = findDistance - monsterRadiuss[i];

            if (distance < targetDistance)
            {
                findMonsterInfoList.Add(new FindMonsterInfo(distance, i));
            }
        }
    }
}