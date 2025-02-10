using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInventoryView : MonoBehaviour
{
    [SerializeField]
    private UIInventorySlot uIInventorySlot;
    [SerializeField]
    private Transform createParent;

    private void Awake()
    {
        CreateInventorySlot();
        // CharactorCombinationPanel.CreateButton.onClick.AddListener(OnClickCreateButton);
    }

    private void CreateInventorySlot()
    {
        var list = DataTableManager.CharactorDataTable.GetCharactorClassList((CharactorClassType.S));

        foreach (var item in list)
        {
            var slot = Instantiate(uIInventorySlot, createParent).GetComponent<UIInventorySlot>();
            slot.SetData(item);
            //slot.enableEvent?.Invoke();
        }
    }
}
