using Unity.Burst;
using Unity.Entities;
using UnityEngine;

public partial struct UIDamageTextSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {

        var deltaTime = SystemAPI.Time.DeltaTime;

        foreach (var (uIDamageTextEntity, transformAuthoring) in SystemAPI.Query<RefRW<UIDamageTextEntity>, RefRW<TransformAuthoring>>())
        {
            uIDamageTextEntity.ValueRW.currentTime += deltaTime;
            var ratio = uIDamageTextEntity.ValueRW.currentTime / uIDamageTextEntity.ValueRO.damageEffectInfo.duration;
            var damageEffectInfo = uIDamageTextEntity.ValueRO;
            transformAuthoring.ValueRW.Position = Vector3.Lerp(damageEffectInfo.position, damageEffectInfo.endPosition, ratio);
            transformAuthoring.ValueRW.LocalScale = Vector3.Lerp(damageEffectInfo.scale, damageEffectInfo.targetScale, ratio);
        }

        //if (currentTime > damageEffectData.Duration)
        //    DestroyUIDamageText();
    }
}
