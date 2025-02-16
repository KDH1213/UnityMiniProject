using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UICombinationEffect : MonoBehaviour
{
    [SerializeField]
    private GameObject animationTarget;

    [SerializeField]
    private TextMeshProUGUI charactorName;

    [SerializeField]
    private ParticleSystem createEffect;

    private readonly string charactorNameFormat = "{0} µÓ¿Â!";

    Queue<int> charactorNameQueue = new Queue<int>();
    Sequence animationSequence;

    private void Awake()
    {
        animationSequence = DOTween.Sequence();
        animationSequence.SetAutoKill(false);
        animationTarget.transform.localScale = Vector3.zero;

        animationSequence.Append(animationTarget.transform.DOScale(Vector3.one, 1f))
            .AppendInterval(0.5f)
            .Append(animationTarget.transform.DOScale(Vector3.zero, 1f))
            .onComplete += () => { EndAnimation(); };
    }

    public void OnStartAnimation(int charactorID)
    {
        charactorNameQueue.Enqueue(charactorID);

        if(charactorNameQueue.Count == 1)
        {
            animationTarget.SetActive(true);
            createEffect.gameObject.SetActive(true);
            createEffect.Play();

            StartAnimation();
        }
    }

    private void StartAnimation()
    {
        int charactorID = charactorNameQueue.Dequeue();
        charactorName.text = string.Format(charactorNameFormat, DataTableManager.CharactorDataTable.Get(charactorID).CharacterName);

        animationSequence.Restart();
        //if (charactorNameQueue.Count == 0)
        //    animationSequence.Play();
    }

    private void EndAnimation()
    {
        animationTarget.SetActive(false);
        createEffect.gameObject.SetActive(false);
        charactorNameQueue.Clear();

        if (charactorNameQueue.Count == 0)
        {
            animationTarget.SetActive(false);
            createEffect.gameObject.SetActive(false);
        }
        else
        {
            StartAnimation();
        }
    }
}
