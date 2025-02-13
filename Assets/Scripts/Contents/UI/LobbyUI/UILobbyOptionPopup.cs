using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UILobbyOptionPopup : MonoBehaviour
{
    [SerializeField]
    private SoundManager soundManager;

    [SerializeField]
    private TextMeshProUGUI soundText;

    private readonly string soundMuteString = "사운드 끄기";
    private readonly string soundOnString = "사운드 켜기";

    private bool isMute = false;

    public void OnSoundMute()
    {
        if(!isMute)
            soundText.text = soundMuteString;
        else
            soundText.text = soundOnString;

        isMute = !isMute;
        soundManager.OnSound(isMute);
    }
}
