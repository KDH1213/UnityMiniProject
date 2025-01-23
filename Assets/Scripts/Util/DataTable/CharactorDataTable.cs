using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharactorData
{
    [field: SerializeField] public string Id { get; set; }
    [field: SerializeField] public string PrefabID { get; set; }
    [field: SerializeField] public CharactorClassType CharactorClassType { get; set; }
    [field: SerializeField] public float AttackRange { get; set; }
    [field: SerializeField] public float AttackSpeed { get; set; }
    [field: SerializeField] public string AttackInfoID { get; set; }

    public GameObject PrefabObject;
    //public override string ToString()
    //{
    //    return $"Type : {Type}\nName : {Name}\nDesc : {Desc}\nValue : {Value}\nCost : {Cost}\nIcon : {Icon}\n";
    //}
}


public class CharactorDataTable : DataTable
{
    private List<List<CharactorData>>  charactorDatas = new List<List<CharactorData>>();

    // private Dictionary<string, CharactorData> charactorNormalTable = new Dictionary<string, CharactorData>();
    // private Dictionary<string, CharactorData> charactorATable = new Dictionary<string, CharactorData>();
    // private Dictionary<string, CharactorData> charactorSTable = new Dictionary<string, CharactorData>();
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

        // charactorNormalTable.Clear();
        // charactorATable.Clear();
        // charactorSTable.Clear();

        var list = LoadCSV<CharactorData>(textAsset.text);

        foreach (var item in list)
        {

            item.PrefabObject = (GameObject)(Resources.Load(string.Format(assetPath, item.PrefabID), typeof(GameObject)));
            if (item.PrefabObject == null)
                continue;

            var charactorFsm = item.PrefabObject.GetComponent<CharactorFSM>();
            charactorFsm.AttackData = DataTableManager.AttackDataTable.Get(item.AttackInfoID);
            charactorFsm.CharactorData = item;

            switch (item.CharactorClassType)
            {
                case CharactorClassType.N:
                    if (!charactorDatas[(int)CharactorClassType.N].Contains(item))
                        charactorDatas[(int)CharactorClassType.N].Add(item);
                    else
                    {
                        Debug.LogError($"중복 키 : {item.Id}");
                    }
                    break;
                case CharactorClassType.A:
                    if (!charactorDatas[(int)CharactorClassType.A].Contains(item))
                        charactorDatas[(int)CharactorClassType.A].Add(item);
                    else
                    {
                        Debug.LogError($"중복 키 : {item.Id}");
                    }
                    break;
                case CharactorClassType.S:
                    if (!charactorDatas[(int)CharactorClassType.S].Contains(item))
                        charactorDatas[(int)CharactorClassType.S].Add(item);
                    else
                    {
                        Debug.LogError($"중복 키 : {item.Id}");
                    }
                    break;
                default:
                    break;
            }

            //switch (item.CharactorClassType)
            //{
            //    case CharactorClassType.N:
            //        if (!charactorNormalTable.ContainsKey(item.Id))
            //            charactorNormalTable.Add(item.Id, item);
            //        else
            //        {
            //            Debug.LogError($"중복 키 : {item.Id}");
            //        }
            //        break;
            //    case CharactorClassType.A:
            //        if (!charactorATable.ContainsKey(item.Id))
            //            charactorATable.Add(item.Id, item);
            //        else
            //        {
            //            Debug.LogError($"중복 키 : {item.Id}");
            //        }
            //        break;
            //    case CharactorClassType.S:
            //        if (!charactorSTable.ContainsKey(item.Id))
            //            charactorSTable.Add(item.Id, item);
            //        else
            //        {
            //            Debug.LogError($"중복 키 : {item.Id}");
            //        }
            //        break;
            //    default:
            //        break;
            //}


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

    public CharactorData GetRandom(CharactorClassType type)
    {
        int count = charactorDatas[(int)type].Count;
        return charactorDatas[(int)type][Random.Range(0, count)];
    }
}
