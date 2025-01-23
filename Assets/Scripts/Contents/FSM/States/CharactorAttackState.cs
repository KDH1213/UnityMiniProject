using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharactorAttackState : CharactorBaseState
{
    [SerializeField] private AttackInfoData attackInfoData;
    [SerializeField] private AttackType attackType;

    private Collider2D attackTarget;

    private float attackTime = 0.5f;

    protected override void Awake()
    {
        base.Awake();
        stateType = CharactorStateType.Attack;
    }

    public override void Enter()
    {
        //currentPosition = monsterTransform.position;
        //moveSpeed = charactorFSM.GetStatValue(StatType.MovementSpeed);
        //GetMovePoint();
        //charactorFSM.Animator.SetBool(DHUtil.MonsterAnimationUtil.hashIsMove, true);

        OnStartAttack();
        CharactorFSM.ChangeState(CharactorStateType.Idle);
    }

    public override void ExcuteUpdate()
    {
    }

    public override void Exit()
    {
        // charactorFSM.Animator.SetBool(DHUtil.MonsterAnimationUtil.hashIsMove, false);
    }

    public void SetAttackTarget(Collider2D target)
    {
        attackTarget = target;
    }

    public void OnStartAttack()
    {
        var createObject = Instantiate(attackInfoData.AttackObjectPrefab);
        createObject.transform.position = attackTarget.transform.position + attackInfoData.CreateOffsetPos;
    }
}
