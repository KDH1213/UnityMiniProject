using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharactorAttackState : CharactorBaseState
{
    [SerializeField] 
    private AttackType attackType;

    private Collider2D attackTarget;
    private AttackData attackData;

    private float attackTime = 0.5f;

    protected override void Awake()
    {
        base.Awake();
        stateType = CharactorStateType.Attack;
        attackData = CharactorFSM.AttackData;
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
            damageInfo.damage = CharactorFSM.CharactorData.Damage;
            damageInfo.debuffType = attackInfo.DebuffType;
            damageInfo.debuffTime = attackInfo.DebuffTime;
            target.OnDamage(ref damageInfo);
        }
        else if(CharactorFSM.AttackData.AttackType == AttackType.Area)
        {
            var areaAttackObject = Instantiate(CharactorFSM.AttackData.PrefabObject, transform.position, Quaternion.identity);
            areaAttackObject.GetComponent<DamagedObject>().Damage = CharactorFSM.CharactorData.Damage;
            areaAttackObject.transform.localScale = Vector3.one * CharactorFSM.AttackData.RealAttackRange;
            return;
        }

        var createObject = Instantiate(CharactorFSM.AttackData.PrefabObject);
        createObject.transform.position = attackTarget.transform.position;
        createObject.transform.localScale = Vector3.one * CharactorFSM.AttackData.RealAttackRange;
        createObject.GetComponent<DamagedObject>().Damage = CharactorFSM.CharactorData.Damage;
    }

}
