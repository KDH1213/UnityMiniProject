using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIMiniGameView : MonoBehaviour
{
    [SerializeField]
    private LobbySceneController lobbySceneController;

    [SerializeField]
    private TextMeshProUGUI refreshCurrencyText;

    [SerializeField]
    private TextMeshProUGUI rouletteCurrencyText;

    private void Awake()
    {
        lobbySceneController.changeValueRefreshCurrencyEvent.AddListener(OnChangeRefreshCurrencyValue);
        lobbySceneController.changeValueRouletteCurrencyEvent.AddListener(OnChangeRouletteCurrencyValue);
    }

    public void OnRefreshRoulette(int value)
    {
        if(lobbySceneController.OnUseRefreshCurrency(value))
        {

        }
    }

    public void OnChangeRefreshCurrencyValue(int value)
    {
        refreshCurrencyText.text = value.ToString();
    }
    public void OnChangeRouletteCurrencyValue(int value)
    {
        rouletteCurrencyText.text = value.ToString();
    }
}
