using CsvHelper;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[System.Serializable]
public class CombinationData
{
    // 캐릭터 이름
    //판매 가격, 재화 타입, 등급
    public int Id { get; set; }

    public int CharacterID { get; set; }
    public string Ingredient { get; set; }

    public Dictionary<int, int> IngredientTable { get; private set; } = new Dictionary<int, int>();
    
    public List<int> IngredientList { get; private set; } = new List<int>();
}

public class CombinationTable : DataTable
{
    private Dictionary<int, CombinationData> combinationDictionoary = new Dictionary<int, CombinationData>();
    private Dictionary<int, List<int>> charactorResultTable = new Dictionary<int, List<int>>();
    private List<CombinationData> combinationList = new List<CombinationData>();

    // public Dictionary<int, List<CombinationData>> table;
    public List<CombinationData> CombinationList { get { return combinationList; } }

    public override void Load(string filename)
    {
        var path = string.Format(FormatPath, filename);

        var textAsset = Resources.Load<TextAsset>(path);
        combinationList = LoadCSV<CombinationData>(textAsset.text);

        foreach (var item in combinationList)
        {
            var ingredientList = item.Ingredient.Split('_').Select(p => int.Parse(p)).ToList();

            foreach (var ingredient in ingredientList)
            {
                if (!item.IngredientTable.ContainsKey(ingredient))
                    item.IngredientTable.Add(ingredient, 1);
                else
                    ++item.IngredientTable[ingredient];

                item.IngredientList.Add(ingredient);

                if(!charactorResultTable.ContainsKey(ingredient))
                {
                    charactorResultTable.Add(ingredient, new List<int>{ item.Id });
                }
                else
                {
                    charactorResultTable[ingredient].Add(item.Id);
                }
            }

            combinationDictionoary.Add(item.Id, item);
        }
    }

    //public List<CombinationData> GetID(int key)
    //{

    //}

    public CombinationData GetKeyData(int key)
    {
        if (!combinationDictionoary.ContainsKey(key))
        {
            Debug.LogError($"{key} None");
            return default;
        }

        return combinationDictionoary[key];
    }

    public CombinationData GetIndexData(int index)
    {
        if (combinationList.Count <= index || 0 > index)
        {
            Debug.LogError($"{index} None");
            return default;
        }

        return combinationList[index];
    }

    public List<int> GetCombinationList(int charactorID)
    {
        if(!charactorResultTable.ContainsKey(charactorID))
            return default;
        else
            return charactorResultTable[charactorID];
    }
}