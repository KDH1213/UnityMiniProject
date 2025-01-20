using System.Collections.Generic;

public abstract class SaveData
{
    public int Version { get; protected set; }

    public abstract SaveData VersionUp();
}

public class SaveDataV1 : SaveData
{
    public string PlayerName = "TEST";

    public SaveDataV1()
    {
        Version = 1;
    }

    public override SaveData VersionUp()
    {
        var data = new SaveDataV2();
        return data;
    }
}
public class SaveDataV2 : SaveData
{
    public string PlayerName = "TEST";

    public SaveDataV2()
    {
        Version = 2;
    }

    public override SaveData VersionUp()
    {
        var data = new SaveDataV3();
        return data;
    }
}

public class SaveDataV3 : SaveDataV2
{
    public List<SaveItemData> itemList = new List<SaveItemData>();

    public SaveDataV3()
    {
        Version = 3;
    }
    public override SaveData VersionUp()
    {
        var data = new SaveDataV3();
        return data;
    }

}