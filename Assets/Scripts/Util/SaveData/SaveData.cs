using System.Collections.Generic;

public abstract class SaveData
{
    public int Version { get; protected set; }

    public abstract SaveData VersionUp();
}

public class SaveDataV1 : SaveData
{
    public Dictionary<int, bool> CharactorUnlockTable = new Dictionary<int, bool>();
    public int RefreshCurrency;
    public int RouletteCurrency;

    public SaveDataV1()
    {
        Version = 1;

        var list = DataTableManager.CharactorDataTable.GetCharactorClassList(CharactorClassType.S);
        foreach (var item in list)
        {
            CharactorUnlockTable.Add(item.Id, false);
        }
    }

    public override SaveData VersionUp()
    {
        var data = new SaveDataV1();
        return data;
    }
}