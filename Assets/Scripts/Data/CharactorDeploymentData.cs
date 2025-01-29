using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharactorDeploymentData", menuName = "GHH/System/CharactorDeploymentData", order = 0)]
public class CharactorDeploymentData : ScriptableObject
{
    [field: SerializeField]
    public int maxDeploymentCount { get; private set; }

    [field: SerializeField]
    public CharactorClassTypeMask OverlappingClassTypeMask;

    [SerializeField] 
    public List<Vector3> deploymentPositionList;
}
