using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalOption : MonoBehaviour
{
    [SerializeField]
    private int targetFrameRate;
    public static float prevTimeScale;

    private void Awake()
    {
        Application.targetFrameRate = targetFrameRate;
    }

    public void OnPause()
    {
        prevTimeScale = Time.timeScale;
        Time.timeScale = 0f;
    }

    public void OnRestart()
    {
        Time.timeScale = prevTimeScale;
    }

    public void OnExitGame()
    {
        Application.Quit();
    }
}
