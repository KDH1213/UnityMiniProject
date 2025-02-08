using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "VfxContainer", menuName = "VFXContainer/VFXContainer", order = 5)]
public class VfxContainerData : ScriptableObject
{
    [SerializedDictionary, SerializeField]
    private SerializedDictionary<int, List<GameObject>> vfxContainerTable = new SerializedDictionary<int, List<GameObject>>();
    public SerializedDictionary<int, List<GameObject>> VfxContainerTable { get { return vfxContainerTable; } }
}
