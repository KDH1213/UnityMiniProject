using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackInfo", menuName = "AttackInfo", order = 3)]
public class AttackInfoData : ScriptableObject
{
    [field: SerializeField] public GameObject AttackObjectPrefab { private set; get; }
    [field: SerializeField] public VfxContainerData VFXCantainer { private set; get; }
    [field: SerializeField] public string VFXKey { private set; get; }
    [field: SerializeField] public Vector3 CreateOffsetPos { private set; get; }
}
