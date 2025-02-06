using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIDrawJewelRoulette : MonoBehaviour
{
    [SerializeField]
    private Image roulette;

    [SerializeField]
    private Image point;

    [SerializeField]
    private Button drawButton;

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

    // Quaternion target;
    // Quaternion startRotation;
    //float currentTime = 0f;
    //bool isRotation = false;
    //bool isStart = false;
    //bool isEnd = false;

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


        var target = Quaternion.Euler(Vector3.forward * Random.Range(0f, 360f));
        var startRotation = roulette.transform.rotation;

        if(startRotation.z < 0f)
        {
            startRotation *= Quaternion.Euler(Vector3.forward * 360f);
        }

        currentTime = 0f;
        while (currentTime < resultTime)
        {
            currentTime += Time.deltaTime;
            roulette.transform.rotation = Quaternion.Lerp(startRotation, target, currentTime / 0.1f);
            yield return new WaitForEndOfFrame();
        }
        // roulette.transform.rotation.z;

        resultAngleEvent?.Invoke(true, charactorClassType);
        coroutine = null;
    }

    public void OnStartRoulette()
    {
        if (coroutine == null)
            coroutine = StartCoroutine(CoRatation());
        else
            return;
    }

    public void OnTest()
    {
        if (coroutine == null)
            coroutine = StartCoroutine(CoRatation());
        else
            return;

        //isRotation = true;
        //isStart = true;
    }

    //private void Update()
    //{
    //    if (isRotation)
    //    {

    //        if(isStart)
    //        {
    //            currentTime += Time.deltaTime;
    //            roulette.transform.rotation *= Quaternion.Euler(Vector3.forward * rotationSpeed);
    //            if (currentTime > defalutRotationTime)
    //            {
    //                isStart = false;
    //                target = Quaternion.Euler(Vector3.forward * Random.Range(0f, 360f));
    //                startRotation = roulette.transform.rotation;
    //            }
    //        }
    //        else
    //        {
    //            if (startRotation.z < 0f)
    //            {
    //                startRotation *= Quaternion.Euler(Vector3.forward * 360f);
    //            }

    //            currentTime = 0f;

    //            currentTime += Time.deltaTime;
    //            roulette.transform.rotation = Quaternion.Lerp(startRotation, target, currentTime / 0.1f);
    //            if (currentTime > 0.1f)
    //            {
    //                isRotation = false;
    //            }

    //        }
    //    }
    //}
}
