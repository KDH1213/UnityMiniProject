using System.Collections;
using TMPro;
using UnityEngine;

public class AddCurrencyEffect : MonoBehaviour
{
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

    private void Start()
    {
        StartCoroutine(CoEffect());
    }

    private IEnumerator CoEffect()
    { 
        float currentTime = 0f;

        Vector3 position = transform.position;
        Vector3 endPosition = transform.position + direction * distance;
        Vector3 scale = transform.localScale;
        var text = GetComponent<TextMeshProUGUI>();
        Color color = text.color;

        while (currentTime < time)
        {
            currentTime += Time.deltaTime;
            var ratio = currentTime / time;
            transform.position = Vector2.Lerp(position, endPosition, ratio);
            transform.localScale = Vector2.Lerp(scale, targetScale, ratio);
            text.color = Color.Lerp(color, targetColor, ratio);
            yield return new WaitForEndOfFrame();
        }

        Destroy(gameObject);
    }
}
