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
    private UIMovementPath uiMovementPathObject;

    [SerializeField]
    private LayerMask targetLayerMask;

    public UnityEvent<CharactorTileController> sellCharactorEvnet;
    public UnityEvent<CharactorTileController> synthesisCharactorEvnet;

    private PointerEventData pointerEventData = new PointerEventData(null);
    private CharactorTileController seleteCharactorTileObject;
    private CharactorTileController endCharactorTileObject;

    private bool isDrag = false;

    [SerializeField]
    private float dragOnTime = 0.2f;
    private float currentDrageTime = 0f;

    private void Awake()
    {
        charactorUIInteraction.sellCharactorEvnet.AddListener(OnSellInteractionCharactor);
        charactorUIInteraction.synthesisCharactorEvnet.AddListener(OnSynthesisCharactor);
    }

    private void Update()
    {
#if UNITY_STANDALONE
        InputDevicePC();
#endif

#if UNITY_ANDROID || UNITY_IOS
        InputDeviceMobile();
#endif
    }

    private void OnFindTarget()
    {
        if (charactorUIInteraction.gameObject.activeSelf && EventSystem.current.IsPointerOverGameObject())
        {
            var targetUI = EventSystem.current.currentSelectedGameObject;

            if (targetUI != null && targetUI.layer == GetLayer.InteractionUI)
            {
                return;
            }
            else
            {
                seleteCharactorTileObject = null;
            }
        }

#if UNITY_STANDALONE
        var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var target = Physics2D.Raycast(mousePosition, transform.forward, 100f, targetLayerMask);
#endif

#if UNITY_ANDROID || UNITY_IOS
       
        var touchPosition = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
        var target = Physics2D.Raycast(touchPosition, transform.forward, 100f, targetLayerMask);
#endif

        if (target.transform == null)
        {
            attackRangeObject.OnDisableObject();
            charactorUIInteraction.gameObject.SetActive(false);
            return;
        }

        seleteCharactorTileObject = target.transform.GetComponent<CharactorTileController>();

    }
    private void OnStartDrag()
    {
        if (seleteCharactorTileObject == null)
        {
            return;
        }

        attackRangeObject.OnActiveObject(seleteCharactorTileObject);
        charactorUIInteraction.gameObject.SetActive(false);

        uiMovementPathObject.gameObject.SetActive(true);
        uiMovementPathObject.SetStartPoint(seleteCharactorTileObject.transform.position);
        uiMovementPathObject.SetDestination(seleteCharactorTileObject.transform.position);

        isDrag = true;
    }

    private void OnEndDrag()
    {
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

    public void OnSellInteractionCharactor()
    {
        seleteCharactorTileObject.OnSellCharactor();
        sellCharactorEvnet?.Invoke(seleteCharactorTileObject);

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
#if UNITY_STANDALONE
    private void InputDevicePC()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnFindTarget();
        }

        if (Input.GetMouseButton(0) && !isDrag)
        {
            currentDrageTime += Time.deltaTime;

            if (currentDrageTime >= dragOnTime)
            {
                OnStartDrag();
            }
        }

        if (isDrag)
        {
            var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var target = Physics2D.Raycast(mousePosition, transform.forward, 100f, targetLayerMask);

            if (target.transform != null)
            {
                endCharactorTileObject = target.transform.GetComponent<CharactorTileController>();

                uiMovementPathObject.SetDestination(endCharactorTileObject.transform.position);
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (isDrag)
            {
                if (endCharactorTileObject == null || seleteCharactorTileObject == endCharactorTileObject)
                {
                    uiMovementPathObject.gameObject.SetActive(false);
                    OnActiveInteractionUI();
                }
                else
                {
                    seleteCharactorTileObject.OnChangeCharactorInfo(endCharactorTileObject);
                    attackRangeObject.OnTargetMove();

                    uiMovementPathObject.gameObject.SetActive(false);
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
#endif

    #if UNITY_ANDROID || UNITY_IOS
    private void InputDeviceMobile()
    {
        if (MultiTouchManager.Instance.IsTouchBegan)
        {
            OnFindTarget();
        }

        if (MultiTouchManager.Instance.IsTouchPress && !isDrag)
        {
            currentDrageTime += Time.deltaTime;

            if (MultiTouchManager.Instance.IsLongPress)
            {
                OnStartDrag();
            }
        }

        if (isDrag)
        {
            var touchPosition = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            var target = Physics2D.Raycast(touchPosition, transform.forward, 100f, targetLayerMask);

            if (target.transform != null)
            {
                endCharactorTileObject = target.transform.GetComponent<CharactorTileController>();

                uiMovementPathObject.SetDestination(endCharactorTileObject.transform.position);
            }
        }

        if (MultiTouchManager.Instance.IsTap)
        {
            if (isDrag)
            {
                if (endCharactorTileObject == null || seleteCharactorTileObject == endCharactorTileObject)
                {
                    uiMovementPathObject.gameObject.SetActive(false);
                    OnActiveInteractionUI();
                }
                else
                {
                    seleteCharactorTileObject.OnChangeCharactorInfo(endCharactorTileObject);
                    attackRangeObject.OnTargetMove();

                    uiMovementPathObject.gameObject.SetActive(false);
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
    #endif

}
