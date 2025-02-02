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

    private readonly string waveFomat = "Wave {0}/{1}";
    private readonly string nextWaveTime = "{0}";
    private readonly string monsterCountFomat = "{0}/{1}";
    private readonly string coinFomat = "코인 :{0}";
    private readonly string jowelFomat = "보석 :{0}";
    private readonly string charactorCountFomat = "{0}/{1}";
    private readonly string changeCreateCoinValue = "{0}";

    private void Awake()
    {
        gameController.coinChangeEvent.AddListener(OnChangeCoinCount);
        gameController.changeCreateCoinValueEvnet.AddListener(OnChangeCreateCoinValue);
        gameController.jowelChangeEvent.AddListener(OnChangeJowel);
    }

    public void OnChangeCoinCount(int coin)
    {
        coinText.text = string.Format(coinFomat, coin.ToString());
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
        if(currentTime != (int)time)
        {
            currentTime = (int)time;
            waveTimeText.text = string.Format(nextWaveTime, currentTime.ToString());
        }
    }

    public void OnChangeJowel(int jowel)
    {
        jewelText.text =  string.Format(jowelFomat, jowel.ToString());
    }

    public void OnChangeCharactorCount(int count, int maxCount)
    {
        charactorCountText.text =  string.Format(charactorCountFomat, count.ToString(), maxCount.ToString());
    }
}
