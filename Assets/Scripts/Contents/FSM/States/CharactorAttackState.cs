using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharactorAttackState : CharactorBaseState
{
    [SerializeField] 
    private AttackType attackType;

    private Collider2D attackTargetCollider;
    private IDamageable attackTarget;
    private AttackData attackData;

    private Vector2 attackPoint;

    private float attackTime = 0.5f;
    private bool isTargetDeath = false;

    private ReinforcedManager reinforcedManager;

    protected override void Awake()
    {
        reinforcedManager = GameObject.FindWithTag(Tags.ReinforcedManager).GetComponent<ReinforcedManager>();

        base.Awake();
        stateType = CharactorStateType.Attack;
        attackData = CharactorFSM.AttackData;
    }

    public override void Enter()
    {

        var direction = attackTargetCollider.transform.position - transform.position;
        if (direction.x < 0f && charactorFSM.IsFlip())
        {
            charactorFSM.OnFlip();
        }
        else if (direction.x > 0f && !charactorFSM.IsFlip())
        {
            charactorFSM.OnFlip();
        }

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
        if(attackTarget != null && attackTarget.IsDead)
        {
            attackPoint = attackTargetCollider.transform.position;
            attackTarget = null;
            attackTargetCollider = null;
            isTargetDeath = true;
        }
    }

    public override void Exit()
    {
        if (charactorFSM.Animator != null)
        {
            charactorFSM.Animator.ResetTrigger(DHUtil.CharactorAnimationUtil.hashIsAttack);
        }
    }

    public void SetAttackTarget(Collider2D target)
    {
        attackTarget = target.GetComponent<IDamageable>();
        attackTargetCollider = target;
        isTargetDeath = false;
    }

    public void OnStartAttack()
    {
        var damage = CharactorFSM.CharactorData.Damage + CharactorFSM.CharactorData.Damage * reinforcedManager.GetReinforcedLevel(CharactorFSM.CharactorData.CharactorClassType);


        if (CharactorFSM.AttackData.AttackType == AttackType.Single)
        {
            if (!isTargetDeath)
            {
                var attackInfo = CharactorFSM.AttackData;

                var damageInfo = new DamageInfo();
                damageInfo.damage = damage;
                damageInfo.debuffType = attackInfo.DebuffType;
                damageInfo.debuffTime = attackInfo.DebuffTime;
                attackTarget.OnDamage(ref damageInfo);
            }
        }
        else if(CharactorFSM.AttackData.AttackType == AttackType.Area)
        {
            var areaAttackObject = Instantiate(CharactorFSM.AttackData.PrefabObject, transform.position, Quaternion.identity);
            areaAttackObject.GetComponent<DamagedObject>().Damage = damage;
            areaAttackObject.transform.localScale = Vector3.one * CharactorFSM.AttackData.RealAttackRange;
            return;
        }

        var createObject = Instantiate(CharactorFSM.AttackData.PrefabObject);
        createObject.transform.position = isTargetDeath ? attackPoint : attackTargetCollider.transform.position;
        createObject.transform.localScale = Vector3.one * CharactorFSM.AttackData.RealAttackRange;
        createObject.GetComponent<DamagedObject>().Damage = damage;
    }

    public void OnEndAttackAnimation()
    {
        if(CharactorFSM.CurrentStateType == CharactorStateType.Attack)
            CharactorFSM.ChangeState(CharactorStateType.Idle);
    }

}
