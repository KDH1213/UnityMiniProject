using System.Collections.Generic;
using UnityEngine;

public enum ItemTypes
{
    Weapon,
    Equip,
    Consumable,
    End
}
public class ItemData
{
    public string Id { get; set; }
    public ItemTypes Type { get; set; }
    public string Name { get; set; }
    public string Desc { get; set; }
    public int Value { get; set; }
    public int Cost { get; set; }
    public string Icon { get; set; }

    public override string ToString()
    {
        return $"Type : {Type}\nName : {Name}\nDesc : {Desc}\nValue : {Value}\nCost : {Cost}\nIcon : {Icon}\n";
    }


    public Sprite IconSprite
    {
        get
        {
            return Resources.Load<Sprite>($"icon/{Icon}");
        }
    }

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

public class ItemTable : DataTable
{
    private Dictionary<string, ItemData> dictionoary = new Dictionary<string, ItemData>();


    public override void Load(string filename)
    {
        var path = string.Format(FormatPath, filename);

        var textAsset = Resources.Load<TextAsset>(path);
        var list = LoadCSV<ItemData>(textAsset.text);

        dictionoary.Clear();

        foreach (var item in list)
        {
            if (!dictionoary.ContainsKey(item.Id))
            {
                dictionoary.Add(item.Id, item);
            }
            else
            {
                Debug.LogError($"Key Duplicated {item.Id}");
            }
        }
    }

    public ItemData Get(string key)
    {
        if (!dictionoary.ContainsKey(key))
        {
            return default;
        }

        return dictionoary[key];
    }
}
