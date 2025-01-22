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

        //foreach (var id in WaveDataTableIds.String)
        //{
        //    var table = new WaveDataTable();
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
#else
        var table = new StringTable();
        var stringTableId = DataTableIds.String[(int)Varibalbes.currentLanguage];
        table.Load(stringTableId);
        tables.Add(stringTableId, table);

          foreach (var id in ItemTableIds.String)
        {
            var table = new ItemTable();
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
