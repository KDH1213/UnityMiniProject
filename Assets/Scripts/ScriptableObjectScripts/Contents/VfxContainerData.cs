using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;


[CreateAssetMenu(fileName = "VfxContainer", menuName = "VFXContainer/VFXContainer", order = 5)]
public class VfxContainerData : ScriptableObject
{
    [SerializeField]
    private SerializedDictionary<string, List<GameObject>> vfxContainerTable = new SerializedDictionary<string, List<GameObject>>();
    public SerializedDictionary<string, List<GameObject>> VfxContainerTable { get { return vfxContainerTable; } }
}
