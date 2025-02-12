using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIEndGameObject : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI resultText;

    [SerializeField]
    private TextMeshProUGUI waveText;

    [SerializeField]
    private TextMeshProUGUI finalWaveText;

    [SerializeField]
    private TextMeshProUGUI compensationText;

    [SerializeField]
    private GameObject[] compensationOnActiveObjects;

    [SerializeField]
    private GameObject restartButton;
    [SerializeField]
    private GameObject skipButton;

    [SerializeField]
    private GameObject[] endEffectActiveObjects;

    [SerializeField]
    private TextMeshProUGUI[] currencyText;

    [SerializeField]
    private GameObject[] currencySlots;

    [SerializeField]
    private bool clearPanel;

    private readonly string waveTextFormat = "{0}";

    private List<int> currencyValueList = new List<int>();
    private Coroutine endGameEventCoroutine = null;

    private void OnEnable()
    {
        var currentWaveLevel = GameObject.FindWithTag(Tags.GameController).GetComponent<GameController>().SpawnSystem.CurrentWaveLevel;
        finalWaveText.text = string.Format(waveTextFormat, currentWaveLevel.ToString());

        if (!clearPanel)
            currentWaveLevel -= 1;
        else
            currentWaveLevel -= 2;

        if(currentWaveLevel < 0)
            currentWaveLevel = 0;

        var waveData = DataTableManager.WaveDataTable.Get(currentWaveLevel);
        currencyValueList = DataTableManager.WaveDataTable.Get(currentWaveLevel).currencyValueList;
        // currencyValueList.Add(waveData.RefreshCurrency);
        // currencyValueList.Add(waveData.RouletteCurrency);

        // 게임 강종 예외처리 해야 함
        SaveLoadManager.Data.RefreshCurrency += waveData.RefreshCurrency;
        SaveLoadManager.Data.RouletteCurrency += waveData.RouletteCurrency;

        if (endGameEventCoroutine == null)
            endGameEventCoroutine = StartCoroutine(CoEndGameEvent());
    }

    private IEnumerator CoEndGameEvent()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        
        resultText.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(0.5f);

        waveText.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(0.5f);
        
        finalWaveText.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(0.5f);

        compensationText.gameObject.SetActive(true);

        for (int i = 0; i < currencyValueList.Count; i++)
        {
            if (currencyValueList[i] == 0)
                continue;

            currencySlots[i].SetActive(true);
            currencyText[i].text = currencyValueList[i].ToString();

            yield return new WaitForSecondsRealtime(0.5f);
        }

        foreach (var compensationGameObject in compensationOnActiveObjects)
        {
            compensationGameObject.SetActive(true);
        }

        yield return new WaitForSecondsRealtime(0.5f);

        restartButton.SetActive(true);

        foreach (var endActiveGameObject in endEffectActiveObjects)
        {
            endActiveGameObject.SetActive(true);
        }
    }

    public void OnSkip()
    {
        resultText.gameObject.SetActive(true);
        waveText.gameObject.SetActive(true);
        finalWaveText.gameObject.SetActive(true);

        compensationText.gameObject.SetActive(true);

        for (int i = 0; i < currencyValueList.Count; i++)
        {
            if (currencyValueList[i] == 0)
                continue;

            currencySlots[i].SetActive(true);
            currencyText[i].text = currencyValueList[i].ToString();
        }

        foreach (var compensationGameObject in compensationOnActiveObjects)
        {
            compensationGameObject.SetActive(true);
        }
        restartButton.SetActive(true);

        foreach (var endActiveGameObject in endEffectActiveObjects)
        {
            endActiveGameObject.SetActive(true);
        }

        skipButton.gameObject.SetActive(false);
    }
}
