using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

[CreateAssetMenu(fileName = "DamageFontEffectData", menuName = "GHH/System/DamageFontEffectData", order = 0)]
public class DamageTextEffectData : ScriptableObject
{
    [field: SerializeField] 
    public Vector3 Direction { get; private set; }

    [field: SerializeField] 
    public Vector3 OffsetPosition { get; private set; }

    [field: SerializeField]
    public float Distance { get; private set; }

    [field: SerializeField]
    public float Duration { get; private set; }

    [field: SerializeField] 
    public float TargetScaleSize { get; private set; }

    [field: SerializeField] 
    public Color TargetColor { get; private set; }
}

public struct DamageTextEffectInfo : IComponentData, IEnableableComponent
{
    public Vector3  direction;
    public Vector3  offsetPosition;
    public float    distance;
    public float    duration;
    public float    targetScaleSize;
    public Color    targetColor;

    public DamageTextEffectInfo(DamageTextEffectData damageTextEffectData)
    {
        direction = damageTextEffectData.Direction;
        offsetPosition = damageTextEffectData.OffsetPosition;
        distance = damageTextEffectData.Distance;
        duration = damageTextEffectData.Duration;
        targetScaleSize = damageTextEffectData.TargetScaleSize;
        targetColor = damageTextEffectData.TargetColor;
    }
}
