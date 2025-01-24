using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GroundSocketPosition", menuName = "GHH/System/GroundSocketPosition", order = 0)]
public class GroundSocketPositionData : ScriptableObject
{
    [SerializeField] 
    public List<Vector3> twoSocketoffsetList;
    [SerializeField] 
    public List<Vector3> threeSocketList;
}
