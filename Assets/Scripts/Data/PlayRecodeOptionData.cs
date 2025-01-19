using UnityEngine;

[System.Serializable]
public class PlayRecodeOptionData : PlayRecodeData
{
    public float masterVolume = 1f;
    public float bgmVolume = 1f;
    public float sfxVolume = 1f;

    public Vector2Int screenResolution = new Vector2Int(1920, 1080);
    public FullScreenMode screenMode = FullScreenMode.FullScreenWindow;
}
