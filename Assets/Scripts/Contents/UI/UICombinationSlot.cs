using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UICombinationSlot : MonoBehaviour
{
    [SerializeField]
    private Button button;
    public Button Button {  get { return button; } }

    [SerializeField]
    private TextMeshProUGUI persentText;

    [SerializeField]
    private Image slotImage;

    public UnityEvent enableEvent;

    private CombinationData combinationData;
    public CombinationData CombinationData { get { return combinationData; } }

    private readonly string persentFormat = "{0}%";

    private void OnEnable()
    {
        enableEvent?.Invoke();
    }

    public void SetData(CombinationData combinationData)
    {
        this.combinationData = combinationData;
        slotImage.sprite = DataTableManager.CharactorDataTable.Get(combinationData.CharacterID).Icon;
    }

    public void OnSetCombinationPersent(int persent)
    {
        persentText.text = string.Format(persentFormat, persent.ToString());
    }
}
