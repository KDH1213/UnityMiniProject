using System.Collections.Generic;
using UnityEngine;

public class CoinDrawData
{
    public int CallID { get; set; }
    public int CallLV { get; set; }

    public float NCallPct {  get; set; }
    public float ACallPct { get; set; }
    public float SCallPct { get; set; }

    public int UpgradeCost { get; set; }

    private List<float> coinDrawList = new List<float>();
    public List<float> CoinDrawList { get { return coinDrawList; } }

    //public override string ToString()
    //{
    //    return $"Type : {Type}\nName : {Name}\nDesc : {Desc}\nValue : {Value}\nCost : {Cost}\nIcon : {Icon}\n";
    //}
}


public class CoinDrawTable : DataTable
{
    private List<CoinDrawData> list = new List<CoinDrawData>();

    public override void Load(string filename)
    {
        var path = string.Format(FormatPath, filename);

        var textAsset = Resources.Load<TextAsset>(path);
        list.Clear();
        list = LoadCSV<CoinDrawData>(textAsset.text);


        foreach (var item in list)
        {
            if (item == null)
            {
                Debug.LogError($"Key Duplicated");
            }
            else
            {
                item.CoinDrawList.Add(item.NCallPct);
                item.CoinDrawList.Add(item.ACallPct);
                item.CoinDrawList.Add(item.SCallPct);
            }
        }
    }

    public CoinDrawData Get(int index)
    {
        if (list.Count <= index)
        {
            return default;
        }

        return list[index];
    }
}