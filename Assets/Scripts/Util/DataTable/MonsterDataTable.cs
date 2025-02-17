using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MonsterData
{
    [field: SerializeField] 
    public int Id { get; set; }
    [field: SerializeField] 
    public MonsterType MonsterType { get; set; }
    [field: SerializeField] 
    public string MonsterPrefabId { get; set; }
    [field: SerializeField] 
    public float MoveSpeed { get; set; }
    [field: SerializeField]
    public float Hp { get; set; }
    [field: SerializeField] 
    public int CoinQty { get; set; }
    [field: SerializeField]
    public int JewelQty { get; set; }

    public GameObject PrefabObject;
    //public override string ToString()
    //{
    //    return $"Type : {Type}\nName : {Name}\nDesc : {Desc}\nValue : {Value}\nCost : {Cost}\nIcon : {Icon}\n";
    //}
}


public class MonsterDataTable : DataTable
{
    private Dictionary<int, MonsterData> monsterTable = new Dictionary<int, MonsterData>();
    private readonly string assetPath = "Prefabs/Monsters/{0}";

    public override void Load(string filename)
    {
        var path = string.Format(FormatPath, filename);

        var textAsset = Resources.Load<TextAsset>(path);
        monsterTable.Clear();
        var list = LoadCSV<MonsterData>(textAsset.text);

        foreach (var item in list)
        {
            item.PrefabObject = (GameObject)(Resources.Load(string.Format(assetPath, item.MonsterPrefabId), typeof(GameObject)));
            // item.PrefabObject.GetComponent<MonsterFSMController>().SetSpriteRenderer();

            if (item.PrefabObject == null)
                continue;

            if (!monsterTable.ContainsKey(item.Id))
                monsterTable.Add(item.Id, item);
            else
            {
                Debug.LogError($"중복 키 : {item.Id}");
            }
        }
    }

    public MonsterData Get(int id)
    {
        if (!monsterTable.ContainsKey(id))
        {
            Debug.LogError($"Find Fail : {id}");
            return default;
        }

        return monsterTable[id];
    }
}

// AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Monsters/.prefab", typeof(GameObject));