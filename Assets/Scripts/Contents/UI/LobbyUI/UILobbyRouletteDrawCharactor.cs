using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILobbyRouletteDrawCharactor : MonoBehaviour
{
    [SerializeField]
    private LobbySceneController lobbySceneController;

    [SerializeField]
    private ParticleSystem unlockSucesseEffect;
    [SerializeField]
    private ParticleSystem allUnlockSucesseEffect;

    [SerializeField]
    private ParticleSystem failEffect;

    [SerializeField]
    private Button refreshButton;

    [SerializeField]
    private GameObject sucesseView;

    [SerializeField]
    private Image charactorIcon;

    [SerializeField]
    private Button drawButton;

    [SerializeField]
    private Image roulette;

    [SerializeField]
    private Slider rouletteSlider;

    [SerializeField]
    [Range(0f, 1f)]
    private float rouletteValue;

    [SerializeField]
    [Range(0.02f, 0.5f)]
    private float minRefreshValue = 0.02f;

    [SerializeField]
    [Range(0.02f, 0.1f)]
    private float maxRefreshValue = 0.5f;

    [SerializeField]
    [Range(0.1f, 0.5f)]
    private float minUseValueRefreshValue = 0.1f;

    [SerializeField]
    [Range(0.1f, 0.5f)]
    private float maxUseValueRefreshValue = 0.5f;

    [SerializeField]
    private float defalutRotationTime = 0.5f;

    [SerializeField]
    private float rotationSpeed = 50f;
    [SerializeField]
    private float resultTime = 0.1f;

    [SerializeField]
    private int drawValue = 1;

    Coroutine coroutine;

    private float targetAngle;
    private bool isOnRandom = false;

    private void Awake()
    {
        // rouletteSlider.value = rouletteValue;
        unlockSucesseEffect.GetComponent<ParticleSystemCallbackListener>()?.endEvent.AddListener(() =>
        {
            unlockSucesseEffect.gameObject.SetActive(false);
            drawButton.interactable = true;
            refreshButton.interactable = true;
            RefreshRoulette(false);
        });

        allUnlockSucesseEffect.GetComponent<ParticleSystemCallbackListener>()?.endEvent.AddListener(() => 
        {
            allUnlockSucesseEffect.gameObject.SetActive(false);
            drawButton.interactable = true;
            refreshButton.interactable = true;
            RefreshRoulette(false);
        });
        failEffect.GetComponent<ParticleSystemCallbackListener>()?.endEvent.AddListener(() => 
        { 
            failEffect.gameObject.SetActive(false);
            drawButton.interactable = true;
            refreshButton.interactable = true;
        });
    }

    private void Start()
    {
        rouletteValue = SaveLoadManager.Data.lobbyRouletteValue;
        rouletteSlider.value = rouletteValue;
        roulette.transform.rotation = Quaternion.Euler(0f, 0f, SaveLoadManager.Data.lobbyRoulettePanelAngle);
        rouletteSlider.transform.localRotation = Quaternion.Euler(0f, 0f, SaveLoadManager.Data.lobbyRouletteLocalAngle);
    }

    public void OnStartDraw(int value)
    {
        if (coroutine == null && lobbySceneController.OnUseRouletteCurrency(value))
            OnStartRoulette();
    }

    private void OnDisable()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            if (!isOnRandom)
                roulette.transform.rotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));
            else
                roulette.transform.rotation = Quaternion.Euler(0f, 0f, targetAngle);

            float successAngle = rouletteValue * 360f;
            if (roulette.transform.rotation.eulerAngles.z < successAngle)
                SuccessDrawCharactor();

            coroutine = null;
        }

        failEffect.Stop();
        failEffect.gameObject.SetActive(false);

        RefreshRoulette(false);
    }


    private void RefreshRoulette(bool useValue)
    {
        var sliderAngle = Random.value * 360f;
        if (useValue)
            rouletteValue = Random.Range(minUseValueRefreshValue, maxUseValueRefreshValue); 
        else
            rouletteValue = Random.Range(minRefreshValue, maxRefreshValue);

        rouletteSlider.value = rouletteValue;
        rouletteSlider.transform.rotation = Quaternion.Euler(0f, 0f, sliderAngle);

        SaveLoadManager.Data.lobbyRouletteValue = rouletteValue;
        SaveLoadManager.Data.lobbyRoulettePanelAngle = roulette.transform.rotation.eulerAngles.z;
        SaveLoadManager.Data.lobbyRouletteLocalAngle = rouletteSlider.transform.localRotation.eulerAngles.z;
        SaveLoadManager.Save();
    }

    public void OnRefreshRoulette(int value)
    {
        if (!lobbySceneController.OnUseRefreshCurrency(value))
            return;

        RefreshRoulette(true);
    }
    private IEnumerator CoRatation()
    {
        float currentTime = 0f;
        isOnRandom = false;

        while (currentTime < defalutRotationTime)
        {
            currentTime += Time.deltaTime;
            roulette.transform.rotation *= Quaternion.Euler(Vector3.forward * rotationSpeed);
            yield return new WaitForEndOfFrame();
        }

        float startAngle = roulette.transform.rotation.eulerAngles.z;
        float currentAngle = Random.Range(0f, 360f);
        targetAngle = startAngle + currentAngle;
        isOnRandom = true;

        currentTime = 0f;
        while (currentTime < resultTime)
        {
            currentTime += Time.deltaTime;
            startAngle = Mathf.Lerp(startAngle, targetAngle, currentTime / resultTime);
            roulette.transform.rotation = Quaternion.Euler(0f, 0f, startAngle);
            yield return new WaitForEndOfFrame();
        }
        float successAngle = rouletteValue * 360f;
        bool isSuccess = (roulette.transform.rotation * rouletteSlider.transform.localRotation).eulerAngles.z < successAngle;
        Debug.Log(isSuccess);
        if (!isSuccess)
        {
            failEffect.gameObject.SetActive(true);
        }
        else
        {
            SuccessDrawCharactor();
        }

        coroutine = null;
        // RefreshRoulette();
    }

    private void SuccessDrawCharactor()
    {
        var unlockTable = SaveLoadManager.Data.CharactorUnlockTable;

        List<int> lockCharactorList = new List<int>();

        foreach (var item in unlockTable)
        {
            if(!item.Value)
                lockCharactorList.Add(item.Key);
        }

        if (lockCharactorList.Count == 0)
        {
            allUnlockSucesseEffect.gameObject.SetActive(true);
            return;
        }

        int seleteIndex = Random.Range(0, lockCharactorList.Count);
        lobbySceneController.OnGetCharactor(lockCharactorList[seleteIndex]);
        charactorIcon.sprite = DataTableManager.CharactorDataTable.Get(lockCharactorList[seleteIndex]).Icon;
        sucesseView.SetActive(true);
        unlockSucesseEffect.gameObject.SetActive(true);
    }

    public void OnStartRoulette()
    {
        coroutine = StartCoroutine(CoRatation());
        drawButton.interactable = false;
        refreshButton.interactable = false;
    }
}
