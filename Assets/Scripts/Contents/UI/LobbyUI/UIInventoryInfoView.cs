using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIInventoryInfoView : MonoBehaviour
{
    [SerializeField]
    private Image charactorIcon;

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

    private void OnDisable()
    {
        gameObject.SetActive(false);
    }

    public void SetData(CharactorData charactorData)
    {
        // charactorIcon.sprite = charactorData.Icon;
        nameText.text = charactorData.CharacterName;
        attackTypeText.text = TypeStringTable.AttackTypeStrings[((int)DataTableManager.AttackDataTable.Get(charactorData.AttackInfoID).AttackType)];
        attackDamageText.text = charactorData.Damage.ToString();
        classText.text = "S";
        attackSpeedText.text = charactorData.AttackSpeed.ToString();

        var list = DataTableManager.CombinationTable.CombinationList;

        foreach (var comb in list)
        {
            if (comb.CharacterID == charactorData.Id)
            {
                for (int i = 0; i < comb.IngredientList.Count; ++i)
                {
                    ingredientImages[i].sprite = DataTableManager.CharactorDataTable.Get(comb.IngredientList[i]).Icon;
                }

                break;
            }
        }

    }
}
