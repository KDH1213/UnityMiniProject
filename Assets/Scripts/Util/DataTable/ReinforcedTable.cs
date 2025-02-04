using CsvHelper;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class ReinforcedData
{
    public int Id { get; set; }

    public CharactorClassType CharactorClassType { get; set; }
    public CurrencyType CurrencyType { get; set; }

    public string Value { get; set; }
    public string DamagePercent { get; set; }

    public List<int> valueList = new List<int>();
    public List<float> damagePercentList = new List<float>();

    public int MaxCount { get { return valueList.Count; } }
}

public class ReinforcedTable : DataTable
{
    private Dictionary<CharactorClassType, ReinforcedData> reinforcedDictionoary = new Dictionary<CharactorClassType, ReinforcedData>();
    public override void Load(string filename)
    {
        var path = string.Format(FormatPath, filename);

        var textAsset = Resources.Load<TextAsset>(path);
        var reinforcedList = LoadCSV<ReinforcedData>(textAsset.text);

        foreach (var item in reinforcedList)
        {
            if(!reinforcedDictionoary.ContainsKey(item.CharactorClassType))
            {
                item.valueList = item.Value.Split('_').Select(p => int.Parse(p)).ToList();
                item.damagePercentList = item.DamagePercent.Split('_').Select(p => float.Parse(p)).ToList();

                for (int i = 0; i < item.damagePercentList.Count; ++i)
                {
                    item.damagePercentList[i] = item.damagePercentList[i] * 0.01f;
                }
                reinforcedDictionoary.Add(item.CharactorClassType, item);
            }
            else
            {
                Debug.LogError($"{item.Id} None");
            }
        }
    }

    public ReinforcedData GetKeyData(CharactorClassType key)
    {
        if (!reinforcedDictionoary.ContainsKey(key))
        {
            Debug.LogError($"{key} None");
            return default;
        }

        return reinforcedDictionoary[key];
    }
}