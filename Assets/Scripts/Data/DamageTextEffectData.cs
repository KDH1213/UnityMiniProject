using System.Collections;
using System.Collections.Generic;
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
