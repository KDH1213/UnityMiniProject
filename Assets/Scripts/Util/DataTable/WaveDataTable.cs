using System.Collections.Generic;
using UnityEngine;

public class WaveData
{
    public string Id { get; set; }
    public string MonsterID { get; set; }

    public int WaveLevel { get; set; }

    public int SpawnCount { get; set; }
    public float SpawnTime { get; set; }

    public float SpawnWaitTime { get; set; }
    public float SpawnInterval { get; set; }
    
    public CurrencyType CurrencyType { get; set; }
    public int StartCurreneyValue { get; set; }
    

    //public override string ToString()
    //{
    //    return $"Type : {Type}\nName : {Name}\nDesc : {Desc}\nVa9lue : {Value}\nCost : {Cost}\nIcon : {Icon}\n";
    //}

    public string StringName
    {
        get
        {
            return DataTableManager.ItemTable.Get(Id).Name;
        }
    }
    public string StringDesc
    {
        get
        {
            return DataTableManager.ItemTable.Get(Id).Desc;
        }
    }
}


public class WaveDataTable : DataTable
{
    private List<WaveData> list = new List<WaveData>();
    public List<WaveData> List { get { return list; } }

    public override void Load(string filename)
    {
        var path = string.Format(FormatPath, filename);

        var textAsset = Resources.Load<TextAsset>(path);
        list.Clear();
        list = LoadCSV<WaveData>(textAsset.text);


        foreach (var item in list)
        {
            if (item == null)
            {
                Debug.LogError($"Key Duplicated {item.Id}");
            }
        }
    }

    public WaveData Get(int index)
    {
        if (list.Count <= index)
        {
            return default;
        }

        return list[index];
    }
}
