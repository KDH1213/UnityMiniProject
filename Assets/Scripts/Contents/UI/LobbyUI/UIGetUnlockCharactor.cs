using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIGetUnlockCharactor : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI charactorName;

    [SerializeField]
    private Image charactorIcon;

    public void SetCharactorID(int id)
    {
        var charactorData = DataTableManager.CharactorDataTable.Get(id);
        charactorName.text = charactorData.CharacterName;
        charactorIcon.sprite = charactorData.Icon;
    }
}
