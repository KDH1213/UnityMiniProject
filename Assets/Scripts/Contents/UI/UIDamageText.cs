using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIDamageText : MonoBehaviour
{
    [SerializeField]
    private Transform target;
    [SerializeField]
    private Vector3 direction;

    [SerializeField]
    private float distance;

    [SerializeField]
    private float time;

    [SerializeField]
    private Vector3 targetScale;

    [SerializeField]
    private Color targetColor;

    [SerializeField]
    private TextMeshProUGUI damageText;
    private void Start()
    {
        StartCoroutine(CoEffect());
    }

    private IEnumerator CoEffect()
    {
        float currentTime = 0f;

        Vector3 position = target.position;
        Vector3 endPosition = target.position + direction * distance;
        Vector3 scale = target.localScale;
        Color color = damageText.color;

        while (currentTime < time)
        {
            currentTime += Time.deltaTime;
            var ratio = currentTime / time;
            target.position = Vector2.Lerp(position, endPosition, ratio);
            target.localScale = Vector2.Lerp(scale, targetScale, ratio);
            damageText.color = Color.Lerp(color, targetColor, ratio);
            yield return new WaitForEndOfFrame();
        }

        Destroy(gameObject);
    }
}
