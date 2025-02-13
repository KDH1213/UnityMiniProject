using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInventoryView : MonoBehaviour
{
    [SerializeField]
    private UIInventorySlot uIInventorySlot;

    [SerializeField]
    private UIInventoryInfoView uiInventoryInfoView;
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
        var toggleGroup = createParent.GetComponent<ToggleGroup>();

        foreach (var item in list)
        {
            var slot = Instantiate(uIInventorySlot, createParent).GetComponent<UIInventorySlot>();
            slot.SetData(item);
            slot.SetUIInventoryViewInfo(uiInventoryInfoView);
            slot.GetComponent<Toggle>().group = toggleGroup;
            //slot.enableEvent?.Invoke();
        }
    }
}
