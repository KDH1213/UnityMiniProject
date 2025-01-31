using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalOption : MonoBehaviour
{
    [SerializeField]
    private int targetFrameRate;


    private GameModeType currentMode = GameModeType.GamePlay;   

    public static float prevTimeScale;

    private void Awake()
    {
        Application.targetFrameRate = targetFrameRate;
        prevTimeScale = Time.timeScale;
    }

    public void OnPause()
    {
        if (currentMode == GameModeType.GamePause)
        {
            Time.timeScale = 0f;
            return;
        }

        currentMode= GameModeType.GamePause;
        prevTimeScale = Time.timeScale;
        Time.timeScale = 0f;
    }

    public void OnRestart()
    {
        currentMode= GameModeType.GamePlay;
        Time.timeScale = prevTimeScale;
    }

    public void OnExitGame()
    {
        Application.Quit();
    }
}
