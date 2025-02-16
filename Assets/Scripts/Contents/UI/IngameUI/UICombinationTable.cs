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

    [SerializeField]
    private UICombinationEffect uICombinationEffect;

    private UICombinationSlot currentSeleteSlot;

    public UnityEvent disableEvent;
    public UnityEvent enableEvent;

    private List<UICombinationSlot> slotList = new List<UICombinationSlot>();

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
        slotList.Sort(comparisons);

        for (int i = 0; i < slotList.Count; i++)
        {
            slotList[i].transform.SetSiblingIndex(i);
        }
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
            slotList.Add(slot);
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
        uICombinationEffect.OnStartAnimation(currentSeleteSlot.CombinationData.CharacterID);

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
            return -result;
        }
        return -lhs.Persent.CompareTo(rhs.Persent); 
    };
}
