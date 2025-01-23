using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AttackData
{
    [field: SerializeField] public string Id { get; set; }
    [field: SerializeField] public AttackType AttackType { get; set; }
    [field: SerializeField] public float Damage { get; set; }
    [field: SerializeField] public float AttackRange { get; set; }
    [field: SerializeField] public DebuffType DebuffType { get; set; }
    [field: SerializeField] public float DebuffTime { get; set; }
    [field: SerializeField] public string VFXId { get; set; }

    public GameObject PrefabObject;
    //public override string ToString()
    //{
    //    return $"Type : {Type}\nName : {Name}\nDesc : {Desc}\nValue : {Value}\nCost : {Cost}\nIcon : {Icon}\n";
    //}
}


public class AttackDataTable : DataTable
{
    private Dictionary<string, AttackData> attackTable = new Dictionary<string, AttackData>();
    private readonly string assetPath = "Prefabs/TempPrefab/{0}";

    public override void Load(string filename)
    {
        var path = string.Format(FormatPath, filename);

        var textAsset = Resources.Load<TextAsset>(path);
        attackTable.Clear();
        var list = LoadCSV<AttackData>(textAsset.text);

        foreach (var item in list)
        {
            item.PrefabObject = (GameObject)(Resources.Load(string.Format(assetPath, item.Id), typeof(GameObject)));
            if (item.PrefabObject == null)
                continue;

            if (!attackTable.ContainsKey(item.Id))
                attackTable.Add(item.Id, item);
            else
            {
                Debug.LogError($"중복 키 : {item.Id}");
            }
        }
    }

    public AttackData Get(string id)
    {
        if (!attackTable.ContainsKey(id))
        {
            Debug.LogError($"Find Fail : {id}");
            return default;
        }

        return attackTable[id];
    }
}
