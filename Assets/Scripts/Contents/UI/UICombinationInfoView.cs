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
            // TODO :: 데이터 테이블 및 사용 리소스가 완성되지 않아 icon을 갱신할 수 없음
            // 해당 리스트 인덱스를 통해 캐릭터 ID가 사용하는 이미지로 grid에 이미지 로드
            //ingredientList[i]
            // iconImages[i].sprite = Resources.Load<Sprite>()
            iconImages[i].gameObject.SetActive(true);
            iconImages[i].SetIconInfo(list[i].Item1, list[i].Item2);
        }

        // 클릭 했을 대 기준 전 그리드 활성화 카운트가 현재 활성화 된 그리드 개수 보다 작은 경우
        // 사용되지 않는 아이콘이 존재할 수 있기 때문에 비활성화
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
