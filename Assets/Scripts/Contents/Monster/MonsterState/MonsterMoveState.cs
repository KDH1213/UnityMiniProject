using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MonsterMoveState : MonsterBaseState
{
    [SerializeField] private Vector2 movePoint;
    private Vector2 moveDirection;
    private Vector2 currentPosition;
    private float moveSpeed;
    private int moveIndex = 0;

    protected void Awake()
    {
        stateType = MonsterStateType.Move;
    }

    public override void Enter()
    {
        currentPosition = transform.position;
        moveSpeed = monsterFSM.GetStatValue(StatType.MovementSpeed);
        GetMovePoint();
        MonsterFSM.Animator.SetBool(DHUtil.MonsterAnimationUtil.hashIsMove, true);
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
        transform.position = (Vector3)currentPosition;

        if ((currentPosition - movePoint).sqrMagnitude < 1f)
        {
            ++moveIndex;
            GetMovePoint();
        }
    }
}
