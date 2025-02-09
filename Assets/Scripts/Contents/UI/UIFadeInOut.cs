using UnityEngine;
using UnityEngine.UI;

public class UIFadeInOut : MonoBehaviour
{
    [SerializeField]
    private Image fadeInOutImage;

    [SerializeField]
    private bool isUnscale;

    [SerializeField]
    private bool isStartFadeOut;

    [SerializeField]
    private float fadeOutDuration;

    private float currentTime;

    private Color currentColor;
    private Color targetColor;

    private void Awake()
    {
        currentColor = fadeInOutImage.color;
        targetColor = currentColor;
    }

    private void Start()
    {
        if(isStartFadeOut)
        {
            targetColor.a = 1;
            currentColor.a = 0;
            fadeInOutImage.color = currentColor;
        }
        else
        {
            targetColor.a = 0;
            currentColor.a = 1;
            fadeInOutImage.color = currentColor;
        }
    }

    private void Update()
    {
        if (isStartFadeOut)
        {
            FadeOut();
        }
        else
        {
            FadeIn();
        }
    }

    private void FadeIn()
    {
        if(isUnscale)
        {
            currentTime += Time.unscaledDeltaTime;
        }
        else
        {
            currentTime += Time.deltaTime;
        }


        fadeInOutImage.color = Color.Lerp(currentColor, targetColor, currentTime / fadeOutDuration);

        if (currentTime >= fadeOutDuration)
        {
            isStartFadeOut = true;
            targetColor.a = 1;
            currentColor.a = 0;
            currentTime = 0f;
        }
    }
    private void FadeOut()
    {
        if (isUnscale)
        {
            currentTime += Time.unscaledDeltaTime;
        }
        else
        {
            currentTime += Time.deltaTime;
        }

        fadeInOutImage.color = Color.Lerp(currentColor, targetColor, currentTime / fadeOutDuration);

        if (currentTime >= fadeOutDuration)
        {
            isStartFadeOut = false;
            targetColor.a = 0;
            currentColor.a = 1;
            currentTime = 0f;
        }
    }
}
