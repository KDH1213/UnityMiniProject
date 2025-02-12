using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIBossAppearedEffect : MonoBehaviour
{
    [SerializeField]
    private Vector3 maxScale;
    [SerializeField]
    private Vector3 minScale;

    [SerializeField]
    private Color minColor;
    [SerializeField]
    private Color maxColor;


    [SerializeField]
    private float time;

    [SerializeField]
    private Transform animationTarget;

    [SerializeField]
    private Image background;

    [SerializeField]
    private float endTime = 3f;
    private float currentTime;

    Sequence animationSequence;
    Sequence backgroundSequence;

    private void Start()
    {
        animationSequence = DOTween.Sequence();
        animationSequence.SetAutoKill(false);
        animationTarget.transform.localScale = Vector3.zero;

        animationSequence.Append(animationTarget.transform.DOScale(maxScale, time));
        animationSequence.SetLoops(-1, LoopType.Yoyo);

        backgroundSequence = DOTween.Sequence();
        backgroundSequence.Append(background.DOColor(maxColor, time)).SetAutoKill(false);
        backgroundSequence.Append(background.DOColor(minColor, time)).SetAutoKill(false);
        backgroundSequence.SetLoops(-1);
    }

    private void OnDisable()
    {
        animationSequence.Pause();
        backgroundSequence.Pause();
    }

    private void OnEnable()
    {
        animationSequence.Play();
        backgroundSequence.Play();
    }

    private void Update()
    {
        currentTime += Time.deltaTime;

        if(currentTime >= endTime)
        {
            EndEffect();
        }
    }

    public void OnStartEffect()
    {
        currentTime = 0f;

        animationTarget.gameObject.SetActive(true);
        background.gameObject.SetActive(true);

        animationTarget.localScale = minScale;
        background.color = minColor;
    }

    private void EndEffect()
    {
        animationTarget.gameObject.SetActive(false);
        background.gameObject.SetActive(false);
    }
}
