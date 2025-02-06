using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIDrawJewelRoulette : MonoBehaviour
{
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

    private void Awake()
    {
        rouletteSlider.value = rouletteValue;
    }

    private void Start()
    {
        var gameController = GameObject.FindWithTag(Tags.GameController).GetComponent<GameController>();
        drawButton.onClick.AddListener(() => gameController.OnStartDrawJewelChractor(drawValue, OnStartRoulette));
    }

    private IEnumerator CoRatation()
    {
        float currentTime = 0f;

        while (currentTime < defalutRotationTime)
        {
            currentTime += Time.deltaTime;
            roulette.transform.rotation *= Quaternion.Euler(Vector3.forward * rotationSpeed);
            yield return new WaitForEndOfFrame();
        }

        float startAngle = roulette.transform.rotation.eulerAngles.z;
        float currentAngle = Random.Range(0f, 360f);
        float targetAngle = startAngle + currentAngle;


        currentTime = 0f;
        while (currentTime < resultTime)
        {
            currentTime += Time.deltaTime;
            startAngle = Mathf.Lerp(startAngle, targetAngle, currentTime / resultTime);
            roulette.transform.rotation = Quaternion.Euler(0f,0f, startAngle);
            yield return new WaitForEndOfFrame();
        }

        float successAngle = rouletteValue * 360f;

        resultAngleEvent?.Invoke((currentAngle < successAngle), charactorClassType);
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
