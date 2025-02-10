using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    private void OnEnable()
    {
        if(charactorData != null)
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

}
