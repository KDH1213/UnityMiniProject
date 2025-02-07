using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UICombinationIcon : MonoBehaviour
{
    [SerializeField]
    private Image icon;

    [SerializeField]
    private TextMeshProUGUI holdingText;

    private readonly string holdingFormat = "{0}";
    private static string none = "�̺���";
    private static string holding = "����";


    public void SetIconInfo(int item1, bool item2)
    {
        // icon.sprite = Resources.Load<Sprite>()

        holdingText.text = item2 ? string.Format(holdingFormat, holding) : string.Format(holdingFormat, none);
    }
}
