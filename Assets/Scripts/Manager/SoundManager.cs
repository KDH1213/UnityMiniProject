using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] 
    private AudioMixer masterMixer;

    private const string masterName = "Master";
    private const string effectName = "SFX";
    private const string bgmName = "BGM";

    private float masterVolume = 0.5f;
    public bool isOnSound = true;


    private void Start()
    {
        masterMixer.SetFloat(masterName, Mathf.Log10(masterVolume) * 20);
        masterMixer.SetFloat(bgmName, Mathf.Log10(masterVolume) * 20);
        masterMixer.SetFloat(effectName, Mathf.Log10(masterVolume) * 20);

        OnSound(isOnSound);
    }

    public void OnSound(bool useSound)
    {
        isOnSound = useSound;

        if (!isOnSound)
            masterMixer.SetFloat(masterName, Mathf.Log10(-80f) * 20f);
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

