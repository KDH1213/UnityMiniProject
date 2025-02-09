using System.Collections.Generic;
using UnityEngine;

public static class DataTableManager
{
    private static readonly Dictionary<string, DataTable> tables = new Dictionary<string, DataTable>();

    static DataTableManager()
    {

#if UNITY_EDITOR
        //foreach(var id in DataTableIds.String)
        //{ 
        //    var table = new StringTable();
        //    table.Load(id);
        //    tables.Add(id, table);
        //}

        //foreach (var id in ItemTableIds.String)
        //{
        //    var table = new ItemTable();
        //    table.Load(id);
        //    tables.Add(id, table);
        //}

        foreach (var id in CoinDrawTableIds.String)
        {
            var table = new CoinDrawTable();
            table.Load(id);
            tables.Add(id, table);
        }

        foreach (var id in WaveDataTableIds.String)
        {
            var table = new WaveDataTable();
            table.Load(id);
            tables.Add(id, table);
        }

        foreach (var id in MonsterDataTableIds.String)
        {
            var table = new MonsterDataTable();
            table.Load(id);
            tables.Add(id, table);
        }

        foreach (var id in AttackTableIds.String)
        {
            var table = new AttackDataTable();
            table.Load(id);
            tables.Add(id, table);
        }
        foreach (var id in CharactorTableIds.String)
        {
            var table = new CharactorDataTable();
            table.Load(id);
            tables.Add(id, table);
        }

        foreach (var id in CharactorSellTableIds.String)
        {
            var table = new CharactorSellTable();
            table.Load(id);
            tables.Add(id, table);
        }

        foreach (var id in CombinationTableIds.String)
        {
            var table = new CombinationTable();
            table.Load(id);
            tables.Add(id, table);
        }


        foreach (var id in ReinforcedTableIds.String)
        {
            var table = new ReinforcedTable();
            table.Load(id);
            tables.Add(id, table);
        }
#else
        foreach (var id in CoinDrawTableIds.String)
        {
            var table = new CoinDrawTable();
            table.Load(id);
            tables.Add(id, table);
        }

        foreach (var id in WaveDataTableIds.String)
        {
            var table = new WaveDataTable();
            table.Load(id);
            tables.Add(id, table);
        }

        foreach (var id in MonsterDataTableIds.String)
        {
            var table = new MonsterDataTable();
            table.Load(id);
            tables.Add(id, table);
        }

        foreach (var id in AttackTableIds.String)
        {
            var table = new AttackDataTable();
            table.Load(id);
            tables.Add(id, table);
        }
        foreach (var id in CharactorTableIds.String)
        {
            var table = new CharactorDataTable();
            table.Load(id);
            tables.Add(id, table);
        }

        foreach (var id in CharactorSellTableIds.String)
        {
            var table = new CharactorSellTable();
            table.Load(id);
            tables.Add(id, table);
        }

        foreach (var id in CombinationTableIds.String)
        {
            var table = new CombinationTable();
            table.Load(id);
            tables.Add(id, table);
        }


        foreach (var id in ReinforcedTableIds.String)
        {
            var table = new ReinforcedTable();
            table.Load(id);
            tables.Add(id, table);
        }
#endif
    }

    public static StringTable StringTable
    {
        get
        {
            return Get<StringTable>(DataTableIds.String[(int)Varibalbes.currentLanguage]);
        }
    }

    public static ItemTable ItemTable
    {
        get
        {
            return Get<ItemTable>(ItemTableIds.String[0]);
        }
    }

    public static WaveDataTable WaveDataTable
    {
        get
        {
            return Get<WaveDataTable>(WaveDataTableIds.String[0]);
        }
    }
    public static CoinDrawTable CoinDrawTable
    {
        get
        {
            return Get<CoinDrawTable>(CoinDrawTableIds.String[0]);
        }
    }
    public static MonsterDataTable MonsterDataTable
    {
        get
        {
            return Get<MonsterDataTable>(MonsterDataTableIds.String[0]);
        }
    }


    public static AttackDataTable AttackDataTable
    {
        get
        {
            return Get<AttackDataTable>(AttackTableIds.String[0]);
        }
    }


    public static CharactorDataTable CharactorDataTable
    {
        get
        {
            return Get<CharactorDataTable>(CharactorTableIds.String[0]);
        }
    }

    public static CharactorSellTable CharactorSellTable
    {
        get
        {
            return Get<CharactorSellTable>(CharactorSellTableIds.String[0]);
        }
    }

    public static CombinationTable CombinationTable
    {
        get
        {
            return Get<CombinationTable>(CombinationTableIds.String[0]);
        }
    }

    public static ReinforcedTable ReinforcedTable
    {
        get
        {
            return Get<ReinforcedTable>(ReinforcedTableIds.String[0]);
        }
    }
    public static T Get<T>(string id) where T : DataTable
    {
        if(!tables.ContainsKey(id))
        {
            Debug.LogError("테이블 없음");
            return default(T);
        }

        return tables[id] as T;
    }
}
