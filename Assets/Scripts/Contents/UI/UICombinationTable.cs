using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICombinationTable : MonoBehaviour
{
    [SerializeField]
    private Transform createParent;

    [SerializeField]
    private GameObject scrollSlotPrefab;

    [SerializeField]
    private UICombinationInfoView CharactorCombinationPanel;

    [SerializeField]
    private CharactorTileManager charactorTileManager;

    private UICombinationSlot currentSeleteSlot;

    private void OnDisable()
    {
        currentSeleteSlot = null;
        CharactorCombinationPanel.SetEmpty();
    }


    private void Awake()
    {
        CreateCombinationSlot();
        CharactorCombinationPanel.CreateButton.onClick.AddListener(OnClickCreateButton);
    }

    private void CreateCombinationSlot()
    {
        var list = DataTableManager.CombinationTable.CombinationList;

        foreach (var item in list)
        {
            var slot = Instantiate(scrollSlotPrefab, createParent).GetComponent<UICombinationSlot>();
            slot.SetData(item);
            slot.enableEvent.AddListener(() => { slot.OnSetCombinationPersent(charactorTileManager.GetHoldingsStatusPercent(item)); });
            slot.Button.onClick.AddListener(() => { currentSeleteSlot = slot; OnClickCombinationSlot(); });
            slot.enableEvent?.Invoke();
        }
    }

    private void OnClickCombinationSlot()
    {
        if(currentSeleteSlot == null)
        {
            CharactorCombinationPanel.SetEmpty();
        }

        CharactorCombinationPanel.SetCombinationData(currentSeleteSlot.CombinationData);
    }

    private void OnClickCreateButton()
    {
        charactorTileManager.OnCreateCombinationCharactor(currentSeleteSlot.CombinationData);

        gameObject.SetActive(false);
    }
    
}
