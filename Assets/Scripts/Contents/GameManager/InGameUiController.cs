using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class InGameUiController : MonoBehaviour
{
    [SerializeField]
    private GameController gameController;

    [SerializeField]
    private TextMeshProUGUI createCoinValueText;

    [SerializeField]
    private TextMeshProUGUI coinText;
    [SerializeField]
    private TextMeshProUGUI waveText;
    [SerializeField]
    private TextMeshProUGUI monsterText;
    [SerializeField]
    private TextMeshProUGUI waveTimeText;
    [SerializeField]
    private TextMeshProUGUI charactorCountText;
    [SerializeField]
    private TextMeshProUGUI jewelText;

    public UnityEvent createFailEvenet;

    private int currentTime = 0;

    private readonly string waveFomat = "Wave {0} / {1}";
    private readonly string nextWaveTime = "{0}";
    private readonly string monsterCountFomat = "{0} / {1}";
    private readonly string coinFomat = "코인 : {0}";
    private readonly string jowelFomat = "보석 : {0}";
    private readonly string charactorCountFomat = "{0} / {1}";
    private readonly string changeCreateCoinValue = "{0}";

    // TODO :: 리펙토링 예정
    // 컴포넌트 하나 추가하여 해당 액션 자체적으로 진행하게 변경
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
    private TextMeshProUGUI reinforcedCoinText;
    [SerializeField]
    private TextMeshProUGUI reinforcedJewelText;

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

    private void Awake()
    {
        gameController.coinChangeEvent.AddListener(OnChangeCoinCount);
        gameController.changeCreateCoinValueEvnet.AddListener(OnChangeCreateCoinValue);
        gameController.jowelChangeEvent.AddListener(OnChangeJowel);
    }

    public void OnChangeCoinCount(int coin)
    {
        coinText.text = string.Format(coinFomat, coin.ToString());
        reinforcedCoinText.text = string.Format(coinFomat, coin.ToString());
    }
    public void OnChangeCreateCoinValue(int coin)
    {
        createCoinValueText.text = string.Format(changeCreateCoinValue, coin.ToString());
    }

    public void OnChangeMonsterCount(int count, int maxCount)
    {
        monsterText.text =  string.Format(monsterCountFomat, count.ToString(), maxCount.ToString());
    }

    public void OnChangeWave(int wave, int maxWave)
    {
        waveText.text =  string.Format(waveFomat, wave.ToString(), maxWave.ToString());
    }

    public void OnChangeWaveTime(float time)
    {
        if (currentTime != (int)time)
        {
            currentTime = (int)time;
            waveTimeText.text = string.Format(nextWaveTime, currentTime.ToString());
        }
    }

    public void OnChangeJowel(int jowel)
    {
        jewelText.text =  string.Format(jowelFomat, jowel.ToString());
        reinforcedJewelText.text = string.Format(jowelFomat, jowel.ToString());
    }

    public void OnChangeCharactorCount(int count, int maxCount)
    {
        charactorCountText.text =  string.Format(charactorCountFomat, count.ToString(), maxCount.ToString());
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
    }
}
