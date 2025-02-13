using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIInventorySlot : MonoBehaviour
{
    [SerializeField]
    private Color unlockColor;
    [SerializeField]
    private Image charactorIcon;
    [SerializeField]
    private GameObject lockImageObject;
    private CharactorData charactorData;

    [SerializeField]
    private Toggle seleteToggle;
    [SerializeField]
    private UIInventoryInfoView uIInventoryInfoView;

    private void OnEnable()
    {
        seleteToggle.isOn = false;
        seleteToggle.onValueChanged?.Invoke(seleteToggle.isOn);

        if (charactorData != null)
        {
            bool isUnlock = SaveLoadManager.Data.CharactorUnlockTable[charactorData.Id];
            lockImageObject.SetActive(!isUnlock);

            if (!isUnlock)
            {
                charactorIcon.color = unlockColor;
            }
            else
            {
                charactorIcon.color = Color.white;
            }
        }
    }

    private void Awake()
    {
        seleteToggle.onValueChanged.AddListener((b) =>
        {
            if (!b)
                return;
            uIInventoryInfoView.SetData(charactorData);
            uIInventoryInfoView.gameObject.SetActive(true);
        });
    }

    public void SetData(CharactorData item)
    {
        charactorData = item;
        charactorIcon.sprite = item.Icon;

        bool isUnlock = SaveLoadManager.Data.CharactorUnlockTable[charactorData.Id];
        lockImageObject.SetActive(!isUnlock);

        if(!isUnlock)
        {
            charactorIcon.color = unlockColor;
        }
        else
        {
            charactorIcon.color = Color.white;
        }
    }

    public void SetUIInventoryViewInfo(UIInventoryInfoView uIInventoryInfoView)
    {
        this.uIInventoryInfoView = uIInventoryInfoView;
    }
}
