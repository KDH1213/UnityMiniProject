using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioMixer masterMixer;


    private readonly string masterName = "Master";
    private readonly string effectName = "SFX";
    private readonly string bgmName = "Music";

    private float masterVolume;
    private bool isOnSound = true;

    private void Awake()
    {
        masterMixer.GetFloat(masterName, out masterVolume);

        masterMixer.GetFloat(effectName, out float effectSoundValue);
        masterMixer.GetFloat(bgmName, out float bgmSoundValue);
    }

    public void OnSound(bool useSound)
    {
        isOnSound = useSound;

        if (!isOnSound)
            masterMixer.SetFloat(masterName, Mathf.Log10(0f) * 20f);
        else
            masterMixer.SetFloat(masterName, Mathf.Log10(masterVolume) * 20f);
    }

    public void OnValueChangedEffectVolume(float volume)
    {
        masterMixer.SetFloat(effectName, Mathf.Log10(volume) * 20f);
    }
    public void OnValueBGMEffectVolume(float volume)
    {
        masterMixer.SetFloat(bgmName, Mathf.Log10(volume) * 20f);
    }
}

