using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class CharactorUIInteraction : MonoBehaviour, IPointerDownHandler
{
    [SerializeField]
    private GameObject saleButton;

    [SerializeField]
    private GameObject synthesisButton;

    public UnityEvent sellCharactorEvnet;
    public UnityEvent synthesisCharactorEvnet;

    private CharactorTileController selectCharactorTileController;

    public void OnSellCharactor()
    {
        selectCharactorTileController.OnSellCharactor();

        if (selectCharactorTileController.CharactorCount == 0)
        {
            selectCharactorTileController.gameObject.SetActive(false);
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
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // EventSystem.current.SetSelectedGameObject(gameObject);
    }
}
