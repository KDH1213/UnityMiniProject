using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharactorAttackState : CharactorBaseState
{
    [SerializeField] 
    private AttackType attackType;

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

    public override void ExecuteUpdate()
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
        if (CharactorFSM.AttackData.AttackType == AttackType.Single)
        {
            var target = attackTarget.GetComponent<IDamageable>();
            var attackInfo = CharactorFSM.AttackData;

            var damageInfo = new DamageInfo();
            damageInfo.damage = attackInfo.Damage;
            damageInfo.debuffType = attackInfo.DebuffType;
            damageInfo.debuffTime = attackInfo.DebuffTime;
            target.OnDamage(ref damageInfo);
        }

        var createObject = Instantiate(CharactorFSM.AttackData.PrefabObject);
        createObject.transform.position = attackTarget.transform.position;
    }

}
