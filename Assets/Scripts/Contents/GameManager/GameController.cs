using System.Collections;
using TMPro;
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
    public MonsterSpawnSystem SpawnSystem { get { return spawnSystem; } }
    [SerializeField] 
    private InGameUiController inGameUiController;
    [SerializeField] 
    private GameTouchManager gameTouchManager;

    [SerializeField]
    private ReinforcedManager reinforcedManager;

    [SerializeField]
    private int createCoin = 20;
    [SerializeField]
    private int currentCoin = 500;
    public int CurrentCoin { get { return currentCoin; } }
    private int currentJewel = 0;
    public int CurrentJewel { get { return currentJewel; } }

    [SerializeField]
    private int maxMonsterCount;
    private int currentMonsterCount = 0;

    public UnityEvent<int, int> changeMonsterEvnet;
    public UnityEvent<int> coinChangeEvent;
    public UnityEvent<int> changeCreateCoinValueEvnet;
    public UnityEvent<int> jewelChangeEvent;
    public UnityEvent gameClearEvent;
    public UnityEvent gameoverEvent;
    public UnityEvent createFailEvenet;
    public UnityEvent drawJewelEvent;

    private bool isOnJewelDraw = false;

    // TODO :: 임시 게임 종료, 클리어 오브젝트 추가, UI, 보상 시스템 기획서 나올시 수정
    [SerializeField] 
    private GameObject gameoverObject;
    [SerializeField] 
    private GameObject clearObject;

    [SerializeField]
    private Transform coinEffectCreatePoint;
    [SerializeField]
    private Transform jewelEffectCreatePoint;

    [SerializeField]
    private GameObject currencyEffectPrefab;

    private void Awake()
    {
        spawnSystem.changeWaveEvent.AddListener(inGameUiController.OnChangeWave);
        spawnSystem.changeWaveTimeEvent.AddListener(inGameUiController.OnChangeWaveTime);

        charactorTileManager.changeCharatorCountEvent.AddListener(inGameUiController.OnChangeCharactorCount);
        changeMonsterEvnet.AddListener(inGameUiController.OnChangeMonsterCount);
        monsterManager.changeMonsterCount.AddListener(OnChangeMonsterCount);

        gameTouchManager.sellCharactorEvnet.AddListener(OnSellCharactor);

        // gameTouchManager.
    }

    private void Start()
    {
        coinChangeEvent?.Invoke(currentCoin);
        jewelChangeEvent?.Invoke(currentJewel);
        changeCreateCoinValueEvnet?.Invoke(createCoin);
    }

    public void OnAddCoin(int coin)
    {
        if (coin == 0)
            return;


        currentCoin += coin;
        coinChangeEvent?.Invoke(currentCoin);
        Instantiate(currencyEffectPrefab, coinEffectCreatePoint).GetComponent<TextMeshProUGUI>().text = coin.ToString();
    }

    public void OnAddJewel(int jowel)
    {
        if (jowel == 0)
            return;

        currentJewel += jowel;
        jewelChangeEvent?.Invoke(currentJewel);
        Instantiate(currencyEffectPrefab, jewelEffectCreatePoint).GetComponent<TextMeshProUGUI>().text = jowel.ToString();
    }

    // TODO :: 에디터 상 캐릭터 생성 버튼과 이벤트 연결
    public void OnCreateCharactor()
    {
        if(createCoin > currentCoin || !charactorTileManager.IsCreateCharactor())
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

    public void OnStartDrawJewelChractor(int jewelValue, UnityAction drawAction)
    {
        if (isOnJewelDraw || (jewelValue > currentJewel || !charactorTileManager.IsCreateCharactor()))
        {
            createFailEvenet?.Invoke();
            return;
        }

        currentJewel -= jewelValue;
        jewelChangeEvent?.Invoke(currentJewel);
        drawAction?.Invoke();
        isOnJewelDraw = true;
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

    public void OnResult(bool isSuccess, CharactorClassType charactorClassType)
    {
        if(isSuccess)
        {
            var createCharactor = Instantiate(DataTableManager.CharactorDataTable.GetRandomDrawCharactor(charactorClassType).PrefabObject);
            charactorTileManager.CreateCharactor(createCharactor.GetComponent<CharactorFSM>());
        }
        else
        {

        }

        isOnJewelDraw = false;
    }

    public void GameClear()
    {
        Time.timeScale = 0f;

        clearObject.SetActive(true);

        gameClearEvent?.Invoke();
    }
    public void GameOver()
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
        var coinDrawList = DataTableManager.CoinDrawTable.Get(reinforcedManager.CurrentReinforcedLevelTypeCall).CoinDrawList;

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
                return DataTableManager.CharactorDataTable.GetRandomDrawCharactor((CharactorClassType)i).PrefabObject;
            }
            else
            {
                randomPos -= coinDrawList[i];
            }
        }

        return DataTableManager.CharactorDataTable.GetRandomDrawCharactor((CharactorClassType)(count - 1)).PrefabObject;
    }

    public void OnSellCharactor(CharactorTileController charactorTileController)
    {
        CharactorClassType type = charactorTileController.CharactorClassType;

        var sellData = DataTableManager.CharactorSellTable.Get(type);

        if(sellData.CurrencyType == CurrencyType.Coin)
        {
            OnAddCoin(sellData.CurrencyValue);
        }
        else if (sellData.CurrencyType == CurrencyType.Jewel)
        {
            OnAddJewel(sellData.CurrencyValue);
        }

        //switch (type)
        //{
        //    case CharactorClassType.N:
        //        OnAddCoin()
        //        break;
        //    case CharactorClassType.A:
        //        break;
        //    case CharactorClassType.S:
        //        break;
        //    default:
        //        break;
        //}
    }

    public GameObject GetCreateSynthesisCharactor(CharactorClassType currentType)
    {
        return Instantiate(DataTableManager.CharactorDataTable.GetRandomDrawCharactor((currentType + 1)).PrefabObject);
    }

    public void AddCurrencyType(CurrencyType currencyType, int startCurreneyValue)
    {
        switch (currencyType)
        {
            case CurrencyType.Coin:
                OnAddCoin(startCurreneyValue);
                break;
            case CurrencyType.Jewel:
                OnAddJewel(startCurreneyValue);
                break;
            default:
                break;
        }
    }

}
