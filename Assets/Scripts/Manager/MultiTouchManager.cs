using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiTouchManager : Singleton<MultiTouchManager>
{
    public bool IsTouchBegan { get; private set; } = false;
    public bool IsTouchPress { get; private set; } = false;
    public bool IsTap { get; private set; } = false;
    public bool IsDoubleTap { get; private set; } = false;
    public bool IsLongPress { get; private set; } = false;

    public bool IsSwipe { get; private set; } = false;
    public Vector2 SwipeDirection {  get; private set; } = Vector2.zero;
    public float Rotate { get; private set; } = 0f;
    public float ZoomInOut { get; private set; } = 0f;

    [SerializeField] 
    private float tapTime = 0.1f;
    [SerializeField] 
    private float doubleTapTime = 0.5f;
    [SerializeField] 
    private float longTapTime = 1f;

    public int TouchCount { get; private set; }

    private int currentTapCount = 0;

    private int primaryFingerId = -1;
    private int prevFingerId = -1;

    private float startTouchTime;
    private float endTouchTime;

    [SerializeField] 
    private float minSwipeDistance = 0.25f; //Inch
    private float minSwipeDistancePixels;

    private Vector2 primaryFingerTouchStart;

    private void Update()
    {
        ResetInput();

        TouchCount = Input.touchCount;

        foreach (var touch in Input.touches)
        {
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    if (primaryFingerId == -1)
                    {
                        IsTouchBegan = true;
                        primaryFingerId = touch.fingerId;
                        primaryFingerTouchStart = touch.position;
                        startTouchTime = Time.time;
                        ++currentTapCount;
                    }
                    break;
                case TouchPhase.Moved:
                case TouchPhase.Stationary:
                    break;
                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    if (TouchCount == 1 && primaryFingerId == touch.fingerId)
                    {
                        if ((touch.position - primaryFingerTouchStart).magnitude > minSwipeDistance)
                        {
                            currentTapCount = 0;
                            SetSwipeDirection();
                        }
                        else
                        {
                            IsDoubleTap = IsCheckDoubleTouch();
                        }
                        endTouchTime = Time.time;
                        prevFingerId = primaryFingerId;
                        primaryFingerId = -1;
                        IsTouchPress = false;
                    }
                    break;
            }
        }

        if (TouchCount == 0)
        {
            CheckTouch();
        }
        else if (TouchCount == 1)
        {
            CheckLongPress();
        }
        else if (TouchCount == 2)
        {
            Touch touch0 = Input.GetTouch(0);
            Touch touch1 = Input.GetTouch(1);

            Vector2 touch0Prev = touch0.position - touch0.deltaPosition;
            Vector2 touch1Prev = touch1.position - touch1.deltaPosition;

            Vector2 currentTouchPosition = touch0.position - touch1.position;

            float prevDistance = (touch0Prev - touch1Prev).magnitude;
            float currentDistance = (currentTouchPosition).magnitude;

            Rotate = Vector2.SignedAngle(touch0.deltaPosition - touch1.deltaPosition, currentTouchPosition);
            ZoomInOut = currentDistance - prevDistance;
        }
    }

    private void ResetInput()
    {
        if(Input.touchCount == 0)
        {
            IsTap = false;
            IsDoubleTap = false;
            IsLongPress = false;
        }

        if(IsTouchBegan)
        {
            IsTouchBegan = false;
            IsTouchPress = true;
        }
        ZoomInOut = 0f;
        Rotate = 0f;
    }

    private bool IsCheckDoubleTouch()
    {
        if (startTouchTime - Time.time < tapTime)
        {
            if (currentTapCount == 2 && startTouchTime - endTouchTime < doubleTapTime)
            {
                currentTapCount = 0;
                return true;
            }
        }

        return false;
    }

    private void CheckTouch()
    {
        if (Input.touchCount == 0 && currentTapCount == 1 
            && Time.time - endTouchTime > tapTime)
        {
            IsTap = true;
            currentTapCount = 0;
        }
    }

    private void CheckLongPress()
    {
        if (Input.touchCount == 1 && !IsLongPress &&
            Time.time - startTouchTime > longTapTime && primaryFingerId == Input.touches[0].fingerId)
        {
            IsLongPress = true;
        }
    }

    private void SetSwipeDirection()
    {
        IsSwipe = true;
        SwipeDirection = (Input.touches[0].position - primaryFingerTouchStart).normalized;

        if(Mathf.Abs(SwipeDirection.x) > Mathf.Abs(SwipeDirection.y))
        {
            SwipeDirection = SwipeDirection.x > 0f ? Vector2.right : Vector2.left;
        }
        else
        {
            SwipeDirection = SwipeDirection.y > 0f ? Vector2.up : Vector2.down;
        }

    }
}
