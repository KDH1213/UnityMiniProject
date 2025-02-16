using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIReinforcedView : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI reinforcedLevelNText;
    [SerializeField]
    private TextMeshProUGUI reinforcedValueNText;
    [SerializeField]
    private TextMeshProUGUI reinforcedLevelAText;
    [SerializeField]
    private TextMeshProUGUI reinforcedValueAText;
    [SerializeField]
    private TextMeshProUGUI reinforcedLevelSText;
    [SerializeField]
    private TextMeshProUGUI reinforcedValueSText;
    [SerializeField]
    private TextMeshProUGUI reinforcedLevelCellText;
    [SerializeField]
    private TextMeshProUGUI reinforcedValueCellText;
    [SerializeField]
    private TextMeshProUGUI[] reinforcedCellPercentTexts;


    [SerializeField]
    private UIButton reinforcedNButton;
    [SerializeField]
    private UIButton reinforcedAButton;
    [SerializeField]
    private UIButton reinforcedSButton;
    [SerializeField]
    private UIButton reinforcedCellButton;


    private readonly string reinforcedValueFomat = "{0}";
    private readonly string reinforcedLevelValue = "Lv.{0}";
    private readonly string reinforcedMaxFomat = "Max";
    private readonly string reinforcedCellPercentFomat = "{0:F2}%";


    private void Awake()
    {
        SetReinforcedCellPercent(0);
    }


    public void OnChangeCharactorClassReinforced(CharactorClassType charactorClassType, int currentLevel)
    {
        var maxCount = DataTableManager.ReinforcedTable.GetKeyData(charactorClassType).MaxCount;

        switch (charactorClassType)
        {
            case CharactorClassType.N:

                if (maxCount == currentLevel)
                {
                    reinforcedLevelNText.text = string.Format(reinforcedLevelValue, reinforcedMaxFomat);
                    reinforcedValueNText.text = string.Format(reinforcedValueFomat, reinforcedMaxFomat);
                    reinforcedNButton.OnInTeractable(false);
                }
                else
                {
                    reinforcedLevelNText.text = string.Format(reinforcedLevelValue, (currentLevel + 1).ToString());
                    reinforcedValueNText.text = string.Format(reinforcedValueFomat, DataTableManager.ReinforcedTable.GetKeyData(charactorClassType).valueList[currentLevel].ToString());
                }

                break;
            case CharactorClassType.A:
                if (maxCount == currentLevel)
                {
                    reinforcedLevelAText.text = string.Format(reinforcedLevelValue, reinforcedMaxFomat);
                    reinforcedValueAText.text = string.Format(reinforcedValueFomat, reinforcedMaxFomat);
                    reinforcedAButton.OnInTeractable(false);
                }
                else
                {
                    reinforcedLevelAText.text = string.Format(reinforcedLevelValue, (currentLevel + 1).ToString());
                    reinforcedValueAText.text = string.Format(reinforcedValueFomat, DataTableManager.ReinforcedTable.GetKeyData(charactorClassType).valueList[currentLevel].ToString());
                }
                break;
            case CharactorClassType.S:
                if (maxCount == currentLevel)
                {
                    reinforcedLevelSText.text = string.Format(reinforcedLevelValue, reinforcedMaxFomat);
                    reinforcedValueSText.text = string.Format(reinforcedValueFomat, reinforcedMaxFomat);
                    reinforcedSButton.OnInTeractable(false);
                }
                else
                {
                    reinforcedLevelSText.text = string.Format(reinforcedLevelValue, (currentLevel + 1).ToString());
                    reinforcedValueSText.text = string.Format(reinforcedValueFomat, DataTableManager.ReinforcedTable.GetKeyData(charactorClassType).valueList[currentLevel].ToString());
                }
                break;
            default:
                break;
        }
    }

    public void OnChangeCoinDrawLevel(int currentLevel)
    {
        var count = DataTableManager.CoinDrawTable.Get(currentLevel).CoinDrawList.Count;
        if (count < currentLevel)
        {
            reinforcedLevelCellText.text = string.Format(reinforcedLevelValue, reinforcedMaxFomat);
            reinforcedValueCellText.text = string.Format(reinforcedValueFomat, reinforcedMaxFomat);
            reinforcedCellButton.OnInTeractable(false);
        }
        else
            reinforcedLevelCellText.text = string.Format(reinforcedLevelValue, (currentLevel + 1).ToString());
        // reinforcedValueCellText.text = string.Format(reinforcedValueFomat, currentLevel);
        SetReinforcedCellPercent(currentLevel);
    }

    private void SetReinforcedCellPercent(int currentLevel)
    {
        var list = DataTableManager.CoinDrawTable.Get(currentLevel).CoinDrawList;
        for (int i = 0; i < (int)CharactorClassType.End; ++i)
        {
            reinforcedCellPercentTexts[i].text = string.Format(reinforcedCellPercentFomat, list[i].ToString());
        }
    }
}
