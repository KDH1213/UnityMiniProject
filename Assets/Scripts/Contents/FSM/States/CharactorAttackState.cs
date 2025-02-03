using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharactorAttackState : CharactorBaseState
{
    [SerializeField] 
    private AttackType attackType;

    private MonsterFSMController attackTarget;
    private AttackData attackData;

    private Vector2 attackPoint;

    private float attackTime = 0.5f;
    private bool isTargetDeath = false;

    protected override void Awake()
    {
        base.Awake();
        stateType = CharactorStateType.Attack;
        attackData = CharactorFSM.AttackData;
    }

    public override void Enter()
    {
        if (charactorFSM.Animator != null)
        {
            charactorFSM.Animator.SetTrigger(DHUtil.CharactorAnimationUtil.hashIsAttack);
        }
        else
        {
            OnStartAttack();
            CharactorFSM.ChangeState(CharactorStateType.Idle);
        }
    }

    public override void ExecuteUpdate()
    {
        if(!isTargetDeath && attackTarget.CurrentStateType == MonsterStateType.Death)
        {
            attackPoint = attackTarget.transform.position;
            attackTarget = null;
            isTargetDeath= true;
        }
    }

    public override void Exit()
    {
        charactorFSM.Animator.ResetTrigger(DHUtil.CharactorAnimationUtil.hashIsAttack);
    }

    public void SetAttackTarget(Collider2D target)
    {
        attackTarget = target.GetComponent<MonsterFSMController>();
        isTargetDeath = false;
    }

    public void OnStartAttack()
    {
        if (CharactorFSM.AttackData.AttackType == AttackType.Single)
        {
            if (!isTargetDeath)
            {
                var target = attackTarget.GetComponent<IDamageable>();
                var attackInfo = CharactorFSM.AttackData;

                var damageInfo = new DamageInfo();
                damageInfo.damage = CharactorFSM.CharactorData.Damage;
                damageInfo.debuffType = attackInfo.DebuffType;
                damageInfo.debuffTime = attackInfo.DebuffTime;
                target.OnDamage(ref damageInfo);
            }
        }
        else if(CharactorFSM.AttackData.AttackType == AttackType.Area)
        {
            var areaAttackObject = Instantiate(CharactorFSM.AttackData.PrefabObject, transform.position, Quaternion.identity);
            areaAttackObject.GetComponent<DamagedObject>().Damage = CharactorFSM.CharactorData.Damage;
            areaAttackObject.transform.localScale = Vector3.one * CharactorFSM.AttackData.RealAttackRange;
            return;
        }

        var createObject = Instantiate(CharactorFSM.AttackData.PrefabObject);
        createObject.transform.position = isTargetDeath ? attackPoint : attackTarget.transform.position;
        createObject.transform.localScale = Vector3.one * CharactorFSM.AttackData.RealAttackRange;
        createObject.GetComponent<DamagedObject>().Damage = CharactorFSM.CharactorData.Damage;
    }

    public void OnEndAttackAnimation()
    {
        if(CharactorFSM.CurrentStateType == CharactorStateType.Attack)
            CharactorFSM.ChangeState(CharactorStateType.Idle);
    }

}
