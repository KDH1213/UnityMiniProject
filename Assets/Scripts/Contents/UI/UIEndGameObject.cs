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
    private GameObject[] endEffectActiveObjects;

    [SerializeField]
    private TextMeshProUGUI currencyText;

    private readonly string waveTextFormat = "{0}";

    private Coroutine endGameEventCoroutine = null;

    private void OnEnable()
    {
        var currentWaveLevel = GameObject.FindWithTag(Tags.GameController).GetComponent<GameController>().SpawnSystem.CurrentWaveLevel;
        finalWaveText.text = string.Format(waveTextFormat, currentWaveLevel.ToString());

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
}
