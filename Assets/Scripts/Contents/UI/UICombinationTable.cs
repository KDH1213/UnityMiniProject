using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UICombinationTable : MonoBehaviour, IPointerClickHandler
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

    public UnityEvent disableEvent;
    public UnityEvent enableEvent;

    private void OnDisable()
    {
        currentSeleteSlot = null;
        CharactorCombinationPanel.SetEmpty();
        disableEvent?.Invoke();
    }

    private void OnEnable()
    {
        enableEvent?.Invoke();
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

    public void OnPointerClick(PointerEventData eventData)
    {
        eventData.selectedObject = gameObject;
    }
}
