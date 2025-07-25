using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharactorAttackState : CharactorBaseState
{
    [SerializeField]
    private VfxContainerData vfxContainerData;

    [SerializeField]
    private GameObject attackVFX;

    [SerializeField] 
    private AttackType attackType;

    private Transform attackTargetTransform;

    private IDamageable attackTarget;
    private AttackData attackData;

    private Vector2 attackPoint;

    private bool isTargetDeath = false;

    private DamagedObjectPool damageObjectPool;

    private DamageInfo damageInfo = new DamageInfo();

    protected override void Awake()
    {
        base.Awake();
        stateType = CharactorStateType.Attack;
        attackData = CharactorFSM.AttackData;
    }

    private void Start()
    {
        damageObjectPool = (DamagedObjectPool)ObjectPoolManager.Instance.ObjectPoolTable[Tags.DamagedObjectPool];
    }

    public override void Enter()
    {

        var direction = attackTargetTransform.transform.position - transform.position;
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
            attackPoint = attackTargetTransform.transform.position;
            attackTarget = null;
            attackTargetTransform = null;
            isTargetDeath = true;
        }
    }

    public override void Exit()
    {
        charactorFSM.Animator.ResetTrigger(DHUtil.CharactorAnimationUtil.hashIsAttack);
    }

    public void SetAttackTarget(Collider2D target)
    {
        attackTarget = target.GetComponent<IDamageable>();
        attackTargetTransform = target.transform;
        isTargetDeath = false;
    }

    public void SetAttackTarget(GameObject target)
    {
        attackTarget = target.GetComponent<IDamageable>();
        attackTargetTransform = target.transform;
        isTargetDeath = false;
    }

    public void OnStartAttack()
    {
        if (charactorFSM.CurrentStateType != CharactorStateType.Attack)
            return;

        var damage = CharactorFSM.CharactorData.Damage + CharactorFSM.reinforcedDamage;

        if (CharactorFSM.AttackData.AttackType == AttackType.Single)
        {
            if (!isTargetDeath)
            {
                var attackInfo = CharactorFSM.AttackData;

                damageInfo.damage = damage;
                damageInfo.debuffType = attackInfo.DebuffType;
                damageInfo.debuffTime = attackInfo.DebuffTime;
                damageInfo.vfxID = attackInfo.VFXId;
                attackTarget.OnDamage(ref damageInfo);
            }
        }
        else
        {
            var createObject = damageObjectPool.GetDamagedObject(attackData.AttackType);
            createObject.SetAttackData(attackData);
            createObject.Damage = damage;

            if (CharactorFSM.AttackData.AttackType == AttackType.Area)
                createObject.transform.position = transform.position;
            else
                createObject.transform.position = isTargetDeath ? attackPoint : attackTargetTransform.transform.position;

            createObject.StartAttack();    
        }
    }

    public void OnEndAttackAnimation()
    {
        if(CharactorFSM.CurrentStateType == CharactorStateType.Attack)
            CharactorFSM.ChangeState(CharactorStateType.Idle);
    }

    public void OnCreateVFX()
    {
        if (charactorFSM.CurrentStateType != CharactorStateType.Attack)
            return;

        attackVFX?.SetActive(true);
        //if (vfxContainerData.VfxContainerTable.ContainsKey(attackData.VFXId))
        //{
        //    foreach (var vfx in vfxContainerData.VfxContainerTable[attackData.VFXId])
        //    {
        //        Instantiate(vfx, attackEffectPoint.position, Quaternion.identity);
        //    }
        //}
    }
    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.yellow;
    //    Gizmos.DrawSphere(transform.position, CharactorFSM.CharactorData.RealAttackRange * 0.5f);
    //}
}
