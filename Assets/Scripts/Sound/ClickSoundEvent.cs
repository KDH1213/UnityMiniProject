using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickSoundEvent : MonoBehaviour
{
    [SerializeField]
    private int soundID;

    public void OnClickEvent()
    {
        SoundManager.Instance.OnSFXPlay(soundID);
    }
}
