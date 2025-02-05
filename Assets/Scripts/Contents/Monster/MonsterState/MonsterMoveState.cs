using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MonsterMoveState : MonsterBaseState, IRouteMove
{
    [SerializeField] 
    private Vector2 movePoint;

    private Transform monsterTransform;
    [SerializeField] 
    Transform monsterSpriteTransform;
    private Vector2 moveDirection;
    private Vector2 currentPosition;
    private float moveSpeed;
    private int moveIndex = 0;

    public Vector2 MovePoint { get { return movePoint; } }
    public Vector2 CurrentPosition { get { return currentPosition; } }
    public Vector2 MoveDirection { get { return moveDirection; } }
    public int MoveIndex { get { return moveIndex; } }

    protected override void Awake()
    {
        base.Awake();
        stateType = MonsterStateType.Move;
        monsterTransform = gameObject.transform;
    }

    public override void Enter()
    {
        currentPosition = monsterTransform.position;
        moveSpeed = monsterFSM.GetStatValue(StatType.MovementSpeed);
        GetMovePoint();
        MonsterFSM.Animator.SetBool(DHUtil.MonsterAnimationUtil.hashIsMove, true);
        Flip();
    }

    public override void ExecuteUpdate()
    {
        Move();
    }

    public override void Exit()
    {
        MonsterFSM.Animator.SetBool(DHUtil.MonsterAnimationUtil.hashIsMove, false);
    }

    private void GetMovePoint()
    {
        monsterFSM.MonsterSpawner.GetMovePoint(ref movePoint, ref moveDirection, ref moveIndex);
    }

    public void Move()
    {
        currentPosition += moveDirection * (moveSpeed * Time.deltaTime);
        monsterTransform.position = currentPosition;

        if (IsCheckResult())
        {
            currentPosition = movePoint;
            monsterTransform.position = movePoint;
            ++moveIndex;
            GetMovePoint();
            Flip();
        }
    }

    private void Flip()
    {
        if (moveDirection.y < 0f && IsFlip())
        {
            OnFlip();
        }
        else if (moveDirection.y > 0f && !IsFlip())
        {
            OnFlip();
        }
    }

    public void OnFlip()
    {
        var scale = monsterSpriteTransform.localScale;
        scale.x *= -1f;
        monsterSpriteTransform.localScale = scale;
    }

    public bool IsFlip()
    {
        return monsterSpriteTransform.localScale.x > 0f ? false : true;
    }

    public bool IsCheckResult()
    {
        Vector2 position;

        if (moveDirection.x > 0f)
            position.x = Mathf.Min(movePoint.x, currentPosition.x);
        else
            position.x = Mathf.Max(movePoint.x, currentPosition.x);

        if (moveDirection.y > 0f)
            position.y = Mathf.Min(movePoint.y, currentPosition.y);
        else
            position.y = Mathf.Max(movePoint.y, currentPosition.y);

        if (movePoint == position)
            return true;

        return false;
    }
}
