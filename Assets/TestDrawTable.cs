using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDrawTable : MonoBehaviour
{
    void Start()
    {
        var itemTable = DataTableManager.CoinDrawTable;

        Debug.Log(itemTable.Get(0).ToString());
    }
}
