using CsvHelper;
using System.Collections.Generic;
using UnityEngine;

public class CharactorSaleTable : DataTable
{
    public class CharactorSaleData
    {
        //판매 가격, 재화 타입, 등급
        public string Id {  get; set; }
        public CharactorClassType CharactorClassType { get; set; }
        public CurrencyType CurrencyType { get; set; }
        public int CurrencyValue { get; set; }
    }

    private Dictionary<CharactorClassType, CharactorSaleData> dictionoary = new Dictionary<CharactorClassType, CharactorSaleData>();

    public override void Load(string filename)
    {
        var path = string.Format(FormatPath, filename);

        var textAsset = Resources.Load<TextAsset>(path);
        var list = LoadCSV<CharactorSaleData>(textAsset.text);

        dictionoary.Clear();

        foreach (var item in list)
        {
            if (!dictionoary.ContainsKey(item.CharactorClassType))
            {
                dictionoary.Add(item.CharactorClassType, item);
            }
            else
            {
                Debug.LogError($"Key Duplicated {item.Id}");
            }
        }
    }

    public CharactorSaleData Get(CharactorClassType key)
    {
        if (!dictionoary.ContainsKey(key))
        {
            Debug.LogError($"{key} None");
            return default;
        }

        return dictionoary[key];
    }
}
