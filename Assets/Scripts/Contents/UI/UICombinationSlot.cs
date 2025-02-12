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

    [SerializeField]
    private Image lockImage;

    public UnityEvent enableEvent;

    private CombinationData combinationData;
    public CombinationData CombinationData { get { return combinationData; } }

    public int Persent { get; private set; }

    private readonly string persentFormat = "{0}%";

    public void SetData(CombinationData combinationData)
    {
        this.combinationData = combinationData;
        slotImage.sprite = DataTableManager.CharactorDataTable.Get(combinationData.CharacterID).Icon;

        if(!SaveLoadManager.Data.CharactorUnlockTable[combinationData.CharacterID])
        {
            button.interactable = false;
            slotImage.color = Color.white * 0.5f;
            persentText.color = Color.white * 0.5f;
            lockImage.gameObject.SetActive(true);
        }
    }

    public void OnSetCombinationPersent(int persent)
    {
        Persent = persent;
        persentText.text = string.Format(persentFormat, persent.ToString());
    }
}
