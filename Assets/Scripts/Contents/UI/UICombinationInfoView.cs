using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UICombinationInfoView : MonoBehaviour
{
    [SerializeField]
    private UICombinationIcon[] iconImages;

    [SerializeField]
    private TextMeshProUGUI nameText;

    [SerializeField]
    private Image resultIconImage;

    [SerializeField]
    private Button combinationButton;
    public Button CreateButton { get { return combinationButton; } }

    [SerializeField]
    private CharactorTileManager charactorTileManager;

    private int activeGridCount = 0;

    private readonly string nameFormat = "{0}";

    private void Awake()
    {
        SetEmpty();
    }

    private void OnDisable()
    {
        SetEmpty();
    }

    public void SetEmpty()
    {
        foreach(var image in iconImages)
        {
            image.gameObject.SetActive(false);
        }
        activeGridCount = 0;
        nameText.gameObject.SetActive(false);
        resultIconImage.gameObject.SetActive(false);
        combinationButton.gameObject.SetActive(false);
    }

    public void SetCombinationData(CombinationData combinationData)
    {
        var ingredientList = combinationData.IngredientList;
        int count = ingredientList.Count;

        int persent = charactorTileManager.GetHoldingsStatusPercent(combinationData);
        var list = charactorTileManager.GetHoldingsStatus(combinationData);

        for (int i = 0; i < count; ++i)
        {
            // TODO :: ������ ���̺� �� ��� ���ҽ��� �ϼ����� �ʾ� icon�� ������ �� ����
            // �ش� ����Ʈ �ε����� ���� ĳ���� ID�� ����ϴ� �̹����� grid�� �̹��� �ε�
            //ingredientList[i]
            // iconImages[i].sprite = Resources.Load<Sprite>()
            iconImages[i].gameObject.SetActive(true);
            iconImages[i].SetIconInfo(list[i].Item1, list[i].Item2);
        }

        // Ŭ�� ���� �� ���� �� �׸��� Ȱ��ȭ ī��Ʈ�� ���� Ȱ��ȭ �� �׸��� ���� ���� ���� ���
        // ������ �ʴ� �������� ������ �� �ֱ� ������ ��Ȱ��ȭ
        for(int i = count; i < activeGridCount; ++i)
        {
            iconImages[i].gameObject.SetActive(false);
        }

        activeGridCount = count;
        nameText.text = string.Format(nameFormat, combinationData.PrefabID);

        nameText.gameObject.SetActive(true);
        resultIconImage.gameObject.SetActive(true);

        if(persent == 100)
            combinationButton.gameObject.SetActive(true);
        else
            combinationButton.gameObject.SetActive(false);
    }
}
