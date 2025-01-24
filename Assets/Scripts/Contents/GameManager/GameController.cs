using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private CharactorTileManager charactorTileManager;
    [SerializeField]
    private MonsterManager monsterManager;
    [SerializeField]
    private MonsterSpawnSystem spawnSystem;
    [SerializeField] 
    private InGameUiController inGameUiController;

    [SerializeField]
    private int createCoin = 20;
    [SerializeField]
    private int currentCoin = 500;
    private int currentJewel = 0;

    [SerializeField]
    private int maxMonsterCount;

    private int currentWave = 0;
    private int currentMonsterCount = 0;

    public UnityEvent<int, int> changeMonsterEvnet;
    public UnityEvent<int> coinChangeEvent;
    public UnityEvent<int> changeCreateCoinValueEvnet;
    public UnityEvent gameClearEvent;
    public UnityEvent gameoverEvent;
    public UnityEvent createFailEvenet;


    // TODO :: 임시 게임 종료, 클리어 오브젝트 추가, UI, 보상 시스템 기획서 나올시 수정
    [SerializeField] 
    private GameObject gameoverObject;
    [SerializeField] 
    private GameObject clearObject;

    private void Awake()
    {
        spawnSystem.changeWaveEvent.AddListener(inGameUiController.OnChangeWave);
        spawnSystem.changeWaveTimeEvent.AddListener(inGameUiController.OnChangeWaveTime);

        changeMonsterEvnet.AddListener(inGameUiController.OnChangeMonsterCount);
        monsterManager.changeMonsterCount.AddListener(OnChangeMonsterCount);
    }

    private void Start()
    {
        coinChangeEvent.Invoke(currentCoin);
    }

    public void AddCoin(int coin)
    {
        currentCoin += coin;
        coinChangeEvent.Invoke(currentCoin);
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
        createCoin += 2;
        coinChangeEvent?.Invoke(currentCoin);
        changeCreateCoinValueEvnet?.Invoke(createCoin);

        var createCharactor = Instantiate(OnRandomCreateCharactor());
        charactorTileManager.CreateCharactor(createCharactor.GetComponent<CharactorFSM>());
    }

    public void OnChangeMonsterCount(int count)
    {
        currentMonsterCount = count;
        changeMonsterEvnet?.Invoke(currentMonsterCount, maxMonsterCount);

        if (currentMonsterCount == maxMonsterCount)
        {
            GameOver();
        }
    }

    public void GameClear()
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

    private GameObject OnRandomCreateCharactor()
    {
        var coinDrawList = DataTableManager.CoinDrawTable.Get(0).CoinDrawList;

        int count = coinDrawList.Count;
        float randomRange = 0f;

        foreach (var coinDraw in coinDrawList)
        {
            randomRange += coinDraw;
        }

        float randomPos = Random.value * randomRange;

        for (int i = 0; i < count; ++i)
        {
            if (randomPos < coinDrawList[i])
            {
                return DataTableManager.CharactorDataTable.GetRandom((CharactorClassType)i).PrefabObject;
            }
            else
            {
                randomPos -= coinDrawList[i];
            }
        }

        return DataTableManager.CharactorDataTable.GetRandom((CharactorClassType)(count - 1)).PrefabObject;
    }
}
