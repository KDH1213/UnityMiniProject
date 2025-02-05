using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIEndGameObject : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI finalWaveText;

    [SerializeField]
    private TextMeshProUGUI currencyText;

    private readonly string waveTextFormat = "{0}";

    private void OnEnable()
    {
        var currentWaveLevel = GameObject.FindWithTag(Tags.GameController).GetComponent<GameController>().SpawnSystem.CurrentWaveLevel;
        finalWaveText.text = string.Format(waveTextFormat, currentWaveLevel.ToString());
    }
}
