using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AttackData
{
    [field: SerializeField]
    public int Id { get; set; }
    [field: SerializeField]
    public string PrefabID { get; set; }
    [field: SerializeField] 
    public AttackType AttackType { get; set; }
    [field: SerializeField] 
    public float AttackRange { get; set; }
    [field: SerializeField] 
    public DebuffType DebuffType { get; set; }
    [field: SerializeField] 
    public float DebuffTime { get; set; }
    [field: SerializeField] 
    public float DebuffProbability { get; set; }
    [field: SerializeField] 
    public int VFXId { get; set; }

    public GameObject PrefabObject;
    public float RealAttackRange;
    //public override string ToString()
    //{
    //    return $"Type : {Type}\nName : {Name}\nDesc : {Desc}\nValue : {Value}\nCost : {Cost}\nIcon : {Icon}\n";
    //}
}


public class AttackDataTable : DataTable
{
    private Dictionary<int, AttackData> attackTable = new Dictionary<int, AttackData>();
    public Dictionary<int, AttackData> AttackTable {  get { return attackTable; } }

    private readonly string assetPath = "Prefabs/TempPrefab/{0}";

    public override void Load(string filename)
    {
        var path = string.Format(FormatPath, filename);

        var textAsset = Resources.Load<TextAsset>(path);
        attackTable.Clear();
        var list = LoadCSV<AttackData>(textAsset.text);

        foreach (var item in list)
        {
            item.PrefabObject = (GameObject)(Resources.Load(string.Format(assetPath, item.PrefabID), typeof(GameObject)));
            if (item.PrefabObject == null)
                continue;

            item.RealAttackRange = (item.AttackRange * 3f) + 3f;
            item.PrefabObject.GetComponent<DamagedObject>().SetAttackData(item);

            if (!attackTable.ContainsKey(item.Id))
                attackTable.Add(item.Id, item);
            else
            {
                Debug.LogError($"중복 키 : {item.Id}");
            }
        }
    }

    public AttackData Get(int id)
    {
        if (!attackTable.ContainsKey(id))
        {
            Debug.LogError($"Find Fail : {id}");
            return default;
        }

        return attackTable[id];
    }
}
