using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class GameTouchManager : MonoBehaviour
{
    [SerializeField]
    private CharactorAttackRangeObject attackRangeObject;

    [SerializeField]
    private CharactorUIInteraction charactorUIInteraction;

    [SerializeField]
    private GameController gameController;

    [SerializeField]
    private LayerMask targetLayerMask;

    public UnityEvent<CharactorTileController> saleCharactorEvnet;
    public UnityEvent<CharactorTileController> synthesisCharactorEvnet;

    private PointerEventData pointerEventData = new PointerEventData(null);
    private CharactorTileController seleteCharactorTileObject;
    private CharactorTileController endCharactorTileObject;

    private bool isDrag = false;

    private float dragOnTime = 0.2f;
    private float currentDrageTime = 0f;

    private void Awake()
    {
        charactorUIInteraction.saleCharactorEvnet.AddListener(OnSaleInteractionCharactor);
        charactorUIInteraction.synthesisCharactorEvnet.AddListener(OnSynthesisCharactor);
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            OnFindTarget();
        }

        if (Input.GetMouseButton(0))
        {
            currentDrageTime += Time.deltaTime;

            if (currentDrageTime >= dragOnTime)
            {
                OnStartDrag();
            }
        }

        if(isDrag)
        {
            var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var target = Physics2D.Raycast(mousePosition, transform.forward, 100f, targetLayerMask);
            
            if(target.transform != null)
                endCharactorTileObject = target.transform.GetComponent<CharactorTileController>();
            //endCharactorTileObject
        }

        if (Input.GetMouseButtonUp(0))
        {
            if(isDrag)
            {
                if(endCharactorTileObject == null || seleteCharactorTileObject == endCharactorTileObject)
                {
                    OnActiveInteractionUI();
                }
                else
                {
                    seleteCharactorTileObject.OnChangeCharactorInfo(endCharactorTileObject);
                    seleteCharactorTileObject = null;
                    endCharactorTileObject = null;
                }
            }
            else
                OnActiveInteractionUI();

            isDrag = false;
            currentDrageTime = 0f;
        }
    }

    private void OnFindTarget()
    {
        if(charactorUIInteraction.gameObject.activeSelf && EventSystem.current.IsPointerOverGameObject())
        {
             var targetUI = EventSystem.current.currentSelectedGameObject;

            if(targetUI != null && targetUI.layer == GetLayer.InteractionUI)
            {
                return;
            }
        }

        var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var target = Physics2D.Raycast(mousePosition,transform.forward, 100f, targetLayerMask);

        if (target.transform == null)
        {
            attackRangeObject.OnDisableObject();
            charactorUIInteraction.gameObject.SetActive(false);
            return;
        }

        seleteCharactorTileObject = target.transform.GetComponent<CharactorTileController>();
   
    }

    private void OnSelete()
    {
        // 선택 했을 때 기준 
        // 공격 범위 출력 X 
        // 
    }
    private void OnStartDrag()
    {
        if (seleteCharactorTileObject == null)
        {
            return;
        }

        attackRangeObject.OnDisableObject();
        charactorUIInteraction.gameObject.SetActive(false);
        isDrag = true;
    }

    private void OnEndDrag()
    {
        if(isDrag)
        {

        }
    }

    private void OnActiveInteractionUI()
    {
        if (seleteCharactorTileObject == null)
        {
            return;
        }

        attackRangeObject.OnActiveObject(seleteCharactorTileObject);
        charactorUIInteraction.gameObject.SetActive(attackRangeObject.gameObject.activeSelf);

        if (charactorUIInteraction.gameObject.activeSelf)
        {
            charactorUIInteraction.SetInteractionTile(seleteCharactorTileObject);
            charactorUIInteraction.transform.position = Camera.main.WorldToScreenPoint(attackRangeObject.transform.position);
        }
    }

    public void OnSaleInteractionCharactor()
    {
        saleCharactorEvnet?.Invoke(seleteCharactorTileObject);
        seleteCharactorTileObject.OnSaleCharactor();

        if (seleteCharactorTileObject.CharactorCount == 0)
        {
            charactorUIInteraction.gameObject.SetActive(false);
            attackRangeObject.OnDisableObject();
            seleteCharactorTileObject = null;
        }
    }

    public void OnSynthesisCharactor()
    {
        synthesisCharactorEvnet?.Invoke(seleteCharactorTileObject);
        seleteCharactorTileObject.OnSynthesisCharactor();
        attackRangeObject.OnDisableObject();

        charactorUIInteraction.gameObject.SetActive(false);
        seleteCharactorTileObject = null;
    }
}
