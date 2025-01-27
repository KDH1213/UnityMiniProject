using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class CharactorUIInteraction : MonoBehaviour, IPointerDownHandler
{
    [SerializeField]
    private GameObject saleButton;

    [SerializeField]
    private GameObject synthesisButton;

    public UnityEvent saleCharactorEvnet;
    public UnityEvent synthesisCharactorEvnet;

    private CharactorTileController selectCharactorTileController;

    public void OnSaleCharactor()
    {
        selectCharactorTileController.OnSaleCharactor();

        if (selectCharactorTileController.CharactorCount == 0)
        {
            selectCharactorTileController.gameObject.SetActive(false);
        }

        saleCharactorEvnet?.Invoke();
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
