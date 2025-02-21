using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharactorData
{
    // 캐릭터 이름 추가
    // 캐릭터 아이콘 sprite 추가
    [field: SerializeField] 
    public int Id { get; set; }
    [field: SerializeField] 
    public string PrefabID { get; set; }
    [field: SerializeField] 
    public CharactorClassType CharactorClassType { get; set; }

    [field: SerializeField]
    public float Damage { get; set; }
    [field: SerializeField]
    public float AttackRange { get; set; }

    [field: SerializeField] 
    public float AttackSpeed { get; set; }
    [field: SerializeField] 
    public int AttackInfoID { get; set; }

    public string CharacterName { get; set; }

    public GameObject PrefabObject;
    public Sprite Icon;
    public float RealAttackRange;
}


public class CharactorDataTable : DataTable
{
    private List<List<CharactorData>>  charactorDataList = new List<List<CharactorData>>();
    private Dictionary<int, CharactorData> charactorTable = new Dictionary<int, CharactorData>();

    private readonly string assetPath = "Prefabs/Charactors/{0}";
    private readonly string iconPath = "Sprites/UI/Icon/{0}_ICon";

    public override void Load(string filename)
    {
        var path = string.Format(FormatPath, filename);

        var textAsset = Resources.Load<TextAsset>(path);
        charactorDataList.Clear();

        charactorDataList = new List<List<CharactorData>>()
        {
            new List<CharactorData>(),
            new List<CharactorData>(),
            new List<CharactorData>(),
        };

        var list = LoadCSV<CharactorData>(textAsset.text);

        foreach (var item in list)
        {
            item.PrefabObject = Resources.Load<GameObject>(string.Format(assetPath, item.PrefabID));
            item.Icon = Resources.Load<Sprite>(string.Format(iconPath, item.Id.ToString()));
            if (item.PrefabObject == null)
                continue;

            item.RealAttackRange = (item.AttackRange * 3f) + 3f;
            var charactorFsm = item.PrefabObject.GetComponent<CharactorFSM>();
            charactorFsm.AttackData = DataTableManager.AttackDataTable.Get(item.AttackInfoID);
            charactorFsm.CharactorData = item;

            if (!charactorDataList[(int)item.CharactorClassType].Contains(item))
            {
                charactorDataList[(int)item.CharactorClassType].Add(item);
                charactorTable.Add(item.Id, item);
            }
            else
            {
                Debug.LogError($"중복 키 : {item.Id}");
            }
        }
    }

    public List<CharactorData> GetCharactorClassList(CharactorClassType type)
    {
        return charactorDataList[(int)type];
    }

    public CharactorData Get(CharactorClassType type, int index)
    {
        if (charactorDataList[(int)type][index] == null)
        {
            return default;
        }
        else
            return charactorDataList[(int)type][index];       
    }

    public CharactorData GetRandomDrawCharactor(CharactorClassType type)
    {
        int count = charactorDataList[(int)type].Count;
        return charactorDataList[(int)type][Random.Range(0, count)];
    }

    public CharactorData Get(int id)
    {
        if(!charactorTable.ContainsKey(id))
            return default;

        return charactorTable[id];
    }
}
