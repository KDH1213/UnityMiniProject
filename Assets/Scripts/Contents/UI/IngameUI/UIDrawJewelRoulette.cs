using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIDrawJewelRoulette : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem successEffect;

    [SerializeField]
    private ParticleSystem failEffect;

    [SerializeField]
    private Image roulette;

    [SerializeField]
    private Button drawButton;

    [SerializeField]
    private Slider rouletteSlider;

    [SerializeField]
    [Range(0f, 1f)]
    private float rouletteValue;

    [SerializeField]
    private float defalutRotationTime = 0.5f;

    [SerializeField]
    private float rotationSpeed = 50f;
    [SerializeField]
    private float resultTime = 0.1f;

    [SerializeField]
    private int drawValue = 1;

    [SerializeField]
    private CharactorClassType charactorClassType;

    public UnityEvent<bool, CharactorClassType> resultAngleEvent;

    Coroutine coroutine;

    private float targetAngle;
    private bool isOnRandom = false;

    private void Awake()
    {
        rouletteSlider.value = rouletteValue;
    }

    private void Start()
    {
        var gameController = GameObject.FindWithTag(Tags.GameController).GetComponent<GameController>();
        drawButton.onClick.AddListener(() => gameController.OnStartDrawJewelChractor(coroutine != null, drawValue, OnStartRoulette));
    }

    private void OnDisable()
    {
        if(coroutine != null)
        {
            StopCoroutine(coroutine);
            if(!isOnRandom)
                roulette.transform.rotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));
            else
                roulette.transform.rotation = Quaternion.Euler(0f, 0f, targetAngle);

            float successAngle = rouletteValue * 360f;
            resultAngleEvent?.Invoke((roulette.transform.rotation.eulerAngles.z < successAngle), charactorClassType);
            coroutine = null;
        }

        failEffect.Stop();
        failEffect.gameObject.SetActive(false);

        successEffect.Stop();
        successEffect.gameObject.SetActive(false);
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
            roulette.transform.rotation = Quaternion.Euler(0f,0f, startAngle);
            yield return new WaitForEndOfFrame();
        }

        float successAngle = rouletteValue * 360f;
        bool isSuccess = roulette.transform.rotation.eulerAngles.z < successAngle;

        if(!isSuccess)
        {
            failEffect.gameObject.SetActive(true);
        }
        else
        {
            successEffect.gameObject.SetActive(true);
        }
        resultAngleEvent?.Invoke(isSuccess, charactorClassType);
        coroutine = null;
    }

    public void OnStartRoulette()
    {
        if (coroutine == null)
            coroutine = StartCoroutine(CoRatation());
        else
            return;
    }

}
