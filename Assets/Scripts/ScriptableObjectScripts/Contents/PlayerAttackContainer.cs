using System;
using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(fileName = "PlayerAttackInfo", menuName = "AttackContainer/PlayerAttackContainer", order = 3)]
[Serializable]
public class PlayerAttackContainer : ScriptableObject
{
    [SerializeField]
    protected SerializedDictionary<string, GameObject> statusTable = new SerializedDictionary<string, GameObject>();
    public SerializedDictionary<string, GameObject> GetStatusTable { get { return statusTable; } }
}
