using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GroundSocketPosition", menuName = "GHH/System/GroundSocketPosition", order = 0)]
public class GroundSocketPositionData : ScriptableObject
{
    [field: SerializeField] public List<List<Vector3>> OffsetList {  get; private set; }
}
