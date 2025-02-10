using UnityEngine;
using UnityEngine.UI;

public class LobbySceneController : MonoBehaviour
{
    [SerializeField]
    private Toggle battleToggle;
    [SerializeField]
    private Toggle storageBoxToggle;
    [SerializeField]
    private Toggle miniGameToggle;

    [SerializeField]
    private Sprite seleteSprite;
    [SerializeField]
    private Sprite defaluteSprite;
    protected void Awake()
    {
        battleToggle.onValueChanged?.Invoke(true);
    }

    public void IsOnBattle(bool isOn)
    {
        if(isOn)
        {
            battleToggle.image.sprite = seleteSprite;
        }
        else
        {
            battleToggle.image.sprite = defaluteSprite;
        }
    }

    public void IsOnStorageBox(bool isOn)
    {
        if (isOn)
        {
            storageBoxToggle.image.sprite = seleteSprite;
        }
        else
        {
            storageBoxToggle.image.sprite = defaluteSprite;
        }
    }

    public void IsOnMiniGame(bool isOn)
    {
        if (isOn)
        {
            miniGameToggle.image.sprite = seleteSprite;
        }
        else
        {
            miniGameToggle.image.sprite = defaluteSprite;
        }
    }

}
