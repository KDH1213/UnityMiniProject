using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class CharactorUIInteraction : MonoBehaviour, IPointerDownHandler
{
    [SerializeField]
    private GameObject saleButton;

    [SerializeField]
    private GameObject synthesisButton;

    [SerializeField]
    private UICharactorInfo uiTargetInfoPanel;

    public UnityEvent sellCharactorEvnet;
    public UnityEvent synthesisCharactorEvnet;

    private CharactorTileController selectCharactorTileController;

    private void OnDisable()
    {
        uiTargetInfoPanel.gameObject.SetActive(false);
    }

    public void OnSellCharactor()
    {
        selectCharactorTileController.OnSellCharactor();

        if (selectCharactorTileController.CharactorCount == 0)
        {
            selectCharactorTileController.gameObject.SetActive(false);
            uiTargetInfoPanel.gameObject.SetActive(false);
        }

        sellCharactorEvnet?.Invoke();
    }

    public void OnSynthesisCharactor()
    {
        if (selectCharactorTileController.IsCreateCharactor())
            return;

        synthesisCharactorEvnet?.Invoke();
    }

    public void SetInteractionTile(CharactorTileController tile)
    {
        selectCharactorTileController = tile;

        if(selectCharactorTileController.IsCreateCharactor())
            synthesisButton.SetActive(false);
        else
            synthesisButton.SetActive(true);

        saleButton.SetActive(true);
        uiTargetInfoPanel.SetData(tile.CharacterControllers[0].CharactorData);
        uiTargetInfoPanel.gameObject.SetActive(true);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // EventSystem.current.SetSelectedGameObject(gameObject);
    }
}
