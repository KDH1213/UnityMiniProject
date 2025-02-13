using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIInventorySlot : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField]
    private Color unlockColor;
    [SerializeField]
    private Image charactorIcon;
    [SerializeField]
    private GameObject lockImageObject;
    private CharactorData charactorData;

    [SerializeField]
    private TextMeshProUGUI nameText;
    [SerializeField]
    private TextMeshProUGUI attackTypeText;
    [SerializeField]
    private TextMeshProUGUI attackDamageText;
    [SerializeField]
    private TextMeshProUGUI attackSpeedText;
    [SerializeField]
    private TextMeshProUGUI classText;

    [SerializeField]
    private Image[] ingredientImages;

    [SerializeField]
    private GameObject infoView;

    private void OnEnable()
    {
        if(charactorData != null)
        {
            bool isUnlock = SaveLoadManager.Data.CharactorUnlockTable[charactorData.Id];
            lockImageObject.SetActive(!isUnlock);

            if (!isUnlock)
            {
                charactorIcon.color = unlockColor;
            }
            else
            {
                charactorIcon.color = Color.white;
            }
        }
    }

    public void SetData(CharactorData item)
    {
        charactorData = item;
        charactorIcon.sprite = item.Icon;

        bool isUnlock = SaveLoadManager.Data.CharactorUnlockTable[charactorData.Id];
        lockImageObject.SetActive(!isUnlock);

        if(!isUnlock)
        {
            charactorIcon.color = unlockColor;
        }
        else
        {
            charactorIcon.color = Color.white;
        }


        nameText.text = charactorData.CharacterName;
        attackTypeText.text = TypeStringTable.AttackTypeStrings[((int)DataTableManager.AttackDataTable.Get(charactorData.AttackInfoID).AttackType)];
        attackDamageText.text = charactorData.Damage.ToString();
        classText.text = "S";
        attackSpeedText.text = charactorData.AttackSpeed.ToString();

        var list = DataTableManager.CombinationTable.CombinationList;
        
        foreach (var comb in list)
        {
            if(comb.CharacterID == item.Id)
            {
                for (int i = 0; i < comb.IngredientList.Count; ++i)
                {
                    ingredientImages[i].sprite = DataTableManager.CharactorDataTable.Get(comb.IngredientList[i]).Icon;
                }
            }
        }

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        infoView.SetActive(true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        infoView.SetActive(false);
    }
}
