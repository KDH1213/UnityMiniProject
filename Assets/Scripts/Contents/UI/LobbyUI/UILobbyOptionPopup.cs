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

    private readonly string soundMuteString = "���� ����";
    private readonly string soundOnString = "���� �ѱ�";

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
