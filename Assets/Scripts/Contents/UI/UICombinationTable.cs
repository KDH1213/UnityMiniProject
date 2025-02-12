using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using static UnityEditor.Progress;

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

    private List<UICombinationSlot> slotList = new List<UICombinationSlot>();
    private List<UICombinationSlot> tempList = new List<UICombinationSlot>();
    private int slotCount = 0;

    private void OnDisable()
    {
        currentSeleteSlot = null;
        CharactorCombinationPanel.SetEmpty();
        disableEvent?.Invoke();
    }

    private void OnEnable()
    {
        enableEvent?.Invoke();

        foreach (var slot in slotList)
        {
            slot.enableEvent?.Invoke();
        }
        //tempList.Sort(comparisons);
        //UpdateSlots(tempList);
    }


    private void Awake()
    {
        CreateCombinationSlot();
        CharactorCombinationPanel.CreateButton.onClick.AddListener(OnClickCreateButton);
    }

    //private void UpdateSlots(List<UICombinationSlot> slots)
    //{
    //    for (int i = 0; i < slotCount; ++i)
    //    {
    //        slotList[i].SetData(slots[i].CombinationData);
    //        slotList[i].OnSetCombinationPersent(slots[i].Persent);
    //    }
    //}

    private void CreateCombinationSlot()
    {
        var list = DataTableManager.CombinationTable.CombinationList;
        int a = 0;
        foreach (var item in list)
        {
            var slot = Instantiate(scrollSlotPrefab, createParent).GetComponent<UICombinationSlot>();
            slot.SetData(item);
            slot.enableEvent.AddListener(() => { slot.OnSetCombinationPersent(charactorTileManager.GetHoldingsStatusPercent(item)); });
            slot.Button.onClick.AddListener(() => { currentSeleteSlot = slot; OnClickCombinationSlot(); });
            slot.enableEvent?.Invoke();
            //slotList.Add(slot);
            //tempList.Add(slot);
        }

        // slotCount = slotList.Count;
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

    private readonly System.Comparison<UICombinationSlot> comparisons = (lhs, rhs) =>
    {
        int result = lhs.Button.interactable.CompareTo(rhs.Button.interactable);
        if (result != 0)
        {
            return result;
        }
        return lhs.Persent.CompareTo(rhs.Persent); 
    };
}
