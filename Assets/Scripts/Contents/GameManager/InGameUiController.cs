using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class InGameUiController : MonoBehaviour
{
    [SerializeField]
    private GameController gameController;

    [field : SerializeField]
    public UIReinforcedView ReinforcedView { get; private set; }

    [SerializeField]
    private TextMeshProUGUI createCoinValueText;

    [SerializeField]
    private TextMeshProUGUI[] coinTexts;
    [SerializeField]
    private TextMeshProUGUI[] charactorCountTexts;
    [SerializeField]
    private TextMeshProUGUI[] jewelTexts;
    [SerializeField]
    private TextMeshProUGUI waveText;
    [SerializeField]
    private TextMeshProUGUI monsterText;
    [SerializeField]
    private TextMeshProUGUI waveTimeText;

    [SerializeField]
    private Slider monsterCountSlider;

    public UnityEvent createFailEvenet;

    private int currentTime = 0;

    private readonly string waveFomat = "{0} / {1}";
    private readonly string nextWaveTime = "½Ã°£ {0}";
    private readonly string monsterCountFomat = "{0} / {1}";
    private readonly string coinFomat = "{0}";
    private readonly string jowelFomat = "{0}";
    private readonly string charactorCountFomat = "{0} / {1}";
    private readonly string changeCreateCoinValue = "{0}";
       
    private void Awake()
    {
        gameController.coinChangeEvent.AddListener(OnChangeCoinCount);
        gameController.changeCreateCoinValueEvnet.AddListener(OnChangeCreateCoinValue);
        gameController.jewelChangeEvent.AddListener(OnChangeJowel);

        monsterCountSlider.value = 0;
    }

    public void OnChangeCoinCount(int coin)
    {
        foreach (var coinText in coinTexts)
        {
            coinText.text = string.Format(coinFomat, coin.ToString());
        }
    }
    public void OnChangeCreateCoinValue(int coin)
    {
        createCoinValueText.text = string.Format(changeCreateCoinValue, coin.ToString());
    }

    public void OnChangeMonsterCount(int count, int maxCount)
    {
        monsterText.text = string.Format(monsterCountFomat, count.ToString(), maxCount.ToString());
        monsterCountSlider.value = (float)count / (float)maxCount;
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
        foreach (var jewelText in jewelTexts)
        {
            jewelText.text = string.Format(jowelFomat, jowel.ToString());
        }
    }

    public void OnChangeCharactorCount(int count, int maxCount)
    {
        foreach (var charactorCountText in charactorCountTexts)
        {
            charactorCountText.text = string.Format(charactorCountFomat, count.ToString(), maxCount.ToString());
        }
    }

}
