using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharactorData
{
    [field: SerializeField] 
    public string Id { get; set; }
    [field: SerializeField] 
    public string PrefabID { get; set; }
    [field: SerializeField] 
    public CharactorClassType CharactorClassType { get; set; }
    [field: SerializeField]
    public float AttackRange { get; set; }
    [field: SerializeField] 
    public float AttackSpeed { get; set; }
    [field: SerializeField] 
    public string AttackInfoID { get; set; }

    public GameObject PrefabObject;
}


public class CharactorDataTable : DataTable
{
    private List<List<CharactorData>>  charactorDatas = new List<List<CharactorData>>();

    private readonly string assetPath = "Prefabs/TempPrefab/{0}";

    public override void Load(string filename)
    {
        var path = string.Format(FormatPath, filename);

        var textAsset = Resources.Load<TextAsset>(path);
        charactorDatas.Clear();

        charactorDatas = new List<List<CharactorData>>()
        {
            new List<CharactorData>(),
            new List<CharactorData>(),
            new List<CharactorData>(),
        };

        var list = LoadCSV<CharactorData>(textAsset.text);

        foreach (var item in list)
        {

            item.PrefabObject = (GameObject)(Resources.Load(string.Format(assetPath, item.PrefabID), typeof(GameObject)));
            if (item.PrefabObject == null)
                continue;

            var charactorFsm = item.PrefabObject.GetComponent<CharactorFSM>();
            charactorFsm.AttackData = DataTableManager.AttackDataTable.Get(item.AttackInfoID);
            charactorFsm.CharactorData = item;

            if (!charactorDatas[(int)item.CharactorClassType].Contains(item))
                charactorDatas[(int)item.CharactorClassType].Add(item);
            else
            {
                Debug.LogError($"중복 키 : {item.Id}");
            }
        }
    }

    public CharactorData Get(CharactorClassType type, int index)
    {
        if (charactorDatas[(int)type][index] == null)
        {
            return default;
        }
        else
            return charactorDatas[(int)type][index];       
    }

    public CharactorData GetRandomDrawCharactor(CharactorClassType type)
    {
        int count = charactorDatas[(int)type].Count;
        return charactorDatas[(int)type][Random.Range(0, count)];
    }
}
