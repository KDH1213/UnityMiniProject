using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private TextMeshProUGUI waveText;

    [SerializeField] private int createMoney = 40;
    [SerializeField] private int currentMoney = 500;
    [SerializeField] private int currentCreateCount = 0;
    private int currentWave = 0;
    [SerializeField] private int maxWave = 80;

    [SerializeField] private MonsterSpawnSystem spawnSystem;

    public UnityEvent<int>      moneyChangeEvent;
    public UnityEvent           gameoverEvent;
    public UnityEvent           createFailEvenet;

    private readonly string waveFomat = string.Format($"{0}/{1}");

    private void Reset()
    {
        currentMoney = 500;
    }

    private void Start()
    {
        moneyText.text = currentMoney.ToString();
    }

    public void AddMoney(int money)
    {
        currentMoney += money;
        moneyText.text = currentMoney.ToString();
        moneyChangeEvent.Invoke(currentMoney);
    }

    public bool UseMoney(int price)
    {
        if(currentMoney < price)
            return false;

        currentMoney -= price;
        moneyText.text = currentMoney.ToString();
        moneyChangeEvent.Invoke(currentMoney);

        return true;
    }

    public void OnCreateCharactor()
    {
        if(createMoney > currentMoney)
        {
            createFailEvenet?.Invoke();
            return;
        }
    }

    public void SetCurrentWave(int wave)
    {
        // waveText.text = string.Format(waveFomat, wave, maxWave);
    }

    private void GameOver()
    {
        Time.timeScale = 0f;
        gameoverEvent?.Invoke();
    }

    public void OnRestart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
        Reset();
    }
}
