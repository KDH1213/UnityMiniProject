using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LobbySceneController : MonoBehaviour
{
    [SerializeField]
    private Toggle battleToggle;
    [SerializeField]
    private Toggle storageBoxToggle;
    [SerializeField]
    private Toggle miniGameToggle;

    [SerializeField]
    private Sprite seleteSprite;
    [SerializeField]
    private Sprite defaluteSprite;

    [SerializeField]
    private GameObject popUpRoulette;

    [SerializeField]
    private int refreshCurrency;
    [SerializeField]
    private int rouletteCurrency;

    public UnityEvent<int> changeValueRefreshCurrencyEvent;
    public UnityEvent<int> changeValueRouletteCurrencyEvent;

    private void Awake()
    {
        battleToggle.onValueChanged?.Invoke(true);

        refreshCurrency = SaveLoadManager.Data.RefreshCurrency;
        rouletteCurrency = SaveLoadManager.Data.RouletteCurrency;
    }

    private void Start()
    {
        changeValueRefreshCurrencyEvent?.Invoke(refreshCurrency);
        changeValueRouletteCurrencyEvent?.Invoke(rouletteCurrency);

        if (rouletteCurrency == 0)
        {
            popUpRoulette.SetActive(true);
        }
    }

    public bool OnUseRefreshCurrency(int value)
    {
        if (refreshCurrency < value)
            return false;

        refreshCurrency -= value;
        SaveLoadManager.Data.RefreshCurrency = refreshCurrency;
        changeValueRefreshCurrencyEvent?.Invoke(refreshCurrency);

        SaveLoadManager.Save(0);

        return true;
    }

    public bool OnUseRouletteCurrency(int value)
    {
        if (rouletteCurrency < value)
            return false;
        rouletteCurrency -= value;
        SaveLoadManager.Data.RouletteCurrency = rouletteCurrency;
        changeValueRouletteCurrencyEvent?.Invoke(rouletteCurrency);

        SaveLoadManager.Save(0);

        return true;
    }

    public void OnGetCharactor(int charactorID)
    {
        Debug.Log(charactorID);
        SaveLoadManager.Data.CharactorUnlockTable[charactorID] = true;
        SaveLoadManager.Save(0);
    }

    public void IsOnBattle(bool isOn)
    {
        if(isOn)
        {
            battleToggle.image.sprite = seleteSprite;
        }
        else
        {
            battleToggle.image.sprite = defaluteSprite;
        }
    }

    public void IsOnStorageBox(bool isOn)
    {
        if (isOn)
        {
            storageBoxToggle.image.sprite = seleteSprite;
        }
        else
        {
            storageBoxToggle.image.sprite = defaluteSprite;
        }
    }

    public void IsOnMiniGame(bool isOn)
    {
        if (isOn)
        {
            miniGameToggle.image.sprite = seleteSprite;
        }
        else
        {
            miniGameToggle.image.sprite = defaluteSprite;
        }
    }

    public void OnPopupNoCoin()
    {
        if (rouletteCurrency == 0)
            popUpRoulette.SetActive(true);
        else
            popUpRoulette.SetActive(false);
    }

}
