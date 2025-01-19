using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(fileName = "StatusInfo", menuName = "StatusInfo", order = 3)]
[System.Serializable]
public class StatusInfoData : ScriptableObject
{
    [SerializeField]
    protected SerializedDictionary<StatType, StatusValue> statusTable = new SerializedDictionary<StatType, StatusValue>();
    public SerializedDictionary<StatType, StatusValue> StatusTable { get { return statusTable; } }

    public StatusValue StatusValue(StatType statType) {  return statusTable[statType]; }
    public float GetStatusValue(StatType statType) { return statusTable[statType].Value; }
}
