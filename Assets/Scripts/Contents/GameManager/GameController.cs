using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField] private CharactorTileManager charactorTileManager;
    [SerializeField] private CharactorFSM characterPrefabs;
    [SerializeField] private MonsterManager monsterManager;
    [SerializeField] private MonsterSpawnSystem spawnSystem;
    [SerializeField] private int maxWave = 80;

    [SerializeField] private int createCoin = 20;
    [SerializeField] private int currentCoin = 500;
    private int currentJewel = 0;

    private int                     currentWave = 0;
    [SerializeField] private int maxMonsterCount;
    private int                     currentMonsterCount = 0;

    public UnityEvent<int>      changeMonsterEvnet;
    public UnityEvent<int>      moneyChangeEvent;
    public UnityEvent           gameClearEvent;
    public UnityEvent           gameoverEvent;
    public UnityEvent           createFailEvenet;

    #region UI Object
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private TextMeshProUGUI monsterText;

    private readonly string waveFomat = "{0}/{1}";
    private readonly string monsterCountFomat = "{0}/{1}";
    #endregion


    [SerializeField] private GameObject gameoverObject;
    [SerializeField] private GameObject clearObject;

    private void Awake()
    {
        monsterManager.changeMonsterCount.AddListener(OnChangeMonsterCount);
    }

    private void Start()
    {
        moneyText.text = currentCoin.ToString();
        monsterText.text = string.Format(monsterCountFomat, currentMonsterCount, maxMonsterCount);
    }

    public void AddCoin(int coin)
    {
        currentCoin += coin;
        moneyText.text = currentCoin.ToString();
        moneyChangeEvent.Invoke(currentCoin);
    }

    public void AddJewel(int jewel)
    {
        currentJewel += jewel;
    }

    public void OnCreateCharactor()
    {
        if(createCoin > currentCoin || charactorTileManager.IsCreateCharactor())
        {
            createFailEvenet?.Invoke();
            return;
        }

        currentCoin -= createCoin;
        moneyText.text = currentCoin.ToString();
        createCoin += 2;

        var createCharactor = Instantiate(characterPrefabs);
        charactorTileManager.CreateCharactor(createCharactor);
    }

    public void SetCurrentWave(int wave)
    {
        // waveText.text = string.Format(waveFomat, wave, maxWave);
    }

    public void OnChangeMonsterCount(int count)
    {
        currentMonsterCount = count;
        monsterText.text = string.Format(monsterCountFomat, currentMonsterCount, maxMonsterCount);

        if(currentMonsterCount == maxMonsterCount)
        {
            GameOver();
        }
    }

    private void GameClear()
    {
        Time.timeScale = 0f;

        clearObject.SetActive(true);
        gameClearEvent?.Invoke();
    }
    private void GameOver()
    {
        Time.timeScale = 0f;

        gameoverObject.SetActive(true);
        gameoverEvent?.Invoke();
    }

    public void OnRestart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}
