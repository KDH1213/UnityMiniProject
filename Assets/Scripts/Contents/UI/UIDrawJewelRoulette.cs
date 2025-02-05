using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDrawJewelRoulette : MonoBehaviour
{
    [SerializeField]
    private Image roulette;

    [SerializeField]
    private Image point;

    [SerializeField]
    private float defalutRotationTime = 0.5f;

    [SerializeField]
    private float rotationSpeed = 20f;

    Coroutine coroutine;
    private IEnumerator CoRatation()
    {
        float currentTime = 0f;

        while (currentTime < defalutRotationTime)
        {
            currentTime += Time.deltaTime;
            roulette.transform.rotation *= Quaternion.Euler(Vector3.forward * rotationSpeed);
            yield return new WaitForEndOfFrame();
        }

        var target = Random.Range(0f, 360f);

        roulette.transform.rotation = Quaternion.Euler(Vector3.forward * target);
        coroutine = null;
    }

    public void OnTest()
    {
        if (coroutine == null)
            coroutine = StartCoroutine(CoRatation());
        else
            return;
    }
}
