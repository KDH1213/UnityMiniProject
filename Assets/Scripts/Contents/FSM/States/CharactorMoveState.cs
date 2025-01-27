using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharactorMoveState : CharactorBaseState
{
    [SerializeField] 
    private Vector2 movePoint;

    private Vector2 startPoint;
    private float currentMoveTime;
    private float moveTime = 0.5f;

    protected override void Awake()
    {
        base.Awake();
        stateType = CharactorStateType.Move;
    }

    public override void Enter()
    {
        currentMoveTime = 0f;

        //currentPosition = monsterTransform.position;
        //moveSpeed = charactorFSM.GetStatValue(StatType.MovementSpeed);
        //GetMovePoint();
        //charactorFSM.Animator.SetBool(DHUtil.MonsterAnimationUtil.hashIsMove, true);
    }

    public override void ExcuteUpdate()
    {
        Move();
    }

    public override void Exit()
    {
        // charactorFSM.Animator.SetBool(DHUtil.MonsterAnimationUtil.hashIsMove, false);
    }

    public void Move()
    {
        currentMoveTime += Time.deltaTime;
        transform.position = Vector2.Lerp(startPoint, movePoint, currentMoveTime / moveTime);

        if(currentMoveTime >= moveTime)
        {
           CharactorFSM.ChangeState(CharactorStateType.Idle);
        }
    }

    private void OnFilp()
    {
    }

    public void OnSetMovePoint(Vector2 movePoint)
    {
        startPoint = transform.position;
        this.movePoint = movePoint;
    }
}
