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
public partial struct FindMonsterJob : IJobParallelFor
{
    [ReadOnly]
    public NativeList<float3> monsterPositions;
    [ReadOnly]
    public NativeList<float> monsterRadiuss;

    [ReadOnly]
    public float3 targetPostion;

    [ReadOnly]
    public float findDistance;
    [ReadOnly]
    public NativeList<FindMonsterInfo> findMonsterInfoList;
    public void Execute(int index)
    {
        int lenght = monsterPositions.Length;

        var distance = math.distancesq(targetPostion, monsterPositions[index]);

        if (distance < findDistance *  + monsterRadiuss[index])
        {
            findMonsterInfoList.Add(new FindMonsterInfo(distance, index));
            // findMonsterInfoList.Sort(new FindMonsterInfoComparer { });
        }
    }

    //public void Execute(int index)
    //{
    //    int lenght = TargetPositions.Length;

    //    float3 seekerPosition = SeekerPositions[index];
    //    float nearestDistanceSq = float.MaxValue;
    //    for (int j = 0; j < lenght; j++)
    //    {
    //        float3 targetPos = TargetPositions[j];
    //        float distanceSq = math.distancesq(seekerPosition, targetPos);
    //        if (distanceSq < nearestDistanceSq)
    //        {
    //            nearestDistanceSq = distanceSq;
    //            NearestTargetPositions[index] = targetPos;
    //        }
    //    }
    //}

}