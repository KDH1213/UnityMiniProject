using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIQuestionView : MonoBehaviour
{
    [SerializeField]
    private GameObject questionPanel;

    [SerializeField]
    private Image[] pageImages;

    [SerializeField]
    private Button nextPageButton;
    [SerializeField]
    private Button previousPageButton;

    private int currentPageIndex;
    private int maxPageCount;

    public UnityEvent enableEvent;
    public UnityEvent disableEvent;

    private void OnEnable()
    {
        enableEvent?.Invoke();
    }

    private void OnDisable()
    {
        disableEvent?.Invoke();
    }

    private void Awake()
    {
        maxPageCount = pageImages.Length;
        currentPageIndex = 0;
        pageImages[0].gameObject.SetActive(true);

        previousPageButton.interactable = false;
    }

    public void OnNextPage()
    {
        pageImages[currentPageIndex].gameObject.SetActive(false);
        ++currentPageIndex;
        pageImages[currentPageIndex].gameObject.SetActive(true);

        if (currentPageIndex >= maxPageCount - 1)
            nextPageButton.interactable = false;

        previousPageButton.interactable = true;
    }
    public void OnPreviousPage()
    {
        pageImages[currentPageIndex].gameObject.SetActive(false);
        --currentPageIndex;
        pageImages[currentPageIndex].gameObject.SetActive(true);

        if (currentPageIndex <= 0)
            previousPageButton.interactable = false;

        nextPageButton.interactable = true;
    }
}
