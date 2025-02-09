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

    private Collider2D attackTargetCollider;
    private IDamageable attackTarget;
    private AttackData attackData;

    private Vector2 attackPoint;

    private float attackTime = 0.5f;
    private bool isTargetDeath = false;

    private ReinforcedManager reinforcedManager;
    private DamagedObjectPool damageObjectPool;

    protected override void Awake()
    {
        reinforcedManager = GameObject.FindWithTag(Tags.ReinforcedManager).GetComponent<ReinforcedManager>();
       // GameObject.FindWithTag(Tags.ReinforcedManager).GetComponent<ReinforcedManager>();

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
        charactorFSM.Animator.ResetTrigger(DHUtil.CharactorAnimationUtil.hashIsAttack);
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
                damageInfo.vfxID = attackInfo.VFXId;
                attackTarget.OnDamage(ref damageInfo);
            }
        }
        else
        {
            var createObject = damageObjectPool.GetDamagedObject(attackData.AttackType);
            var damagedObject = createObject.GetComponent<DamagedObject>();
            damagedObject.SetAttackData(attackData);
            damagedObject.Damage = damage;

            if (CharactorFSM.AttackData.AttackType == AttackType.Area)
                damagedObject.transform.position = transform.position;
            else
                createObject.transform.position = isTargetDeath ? attackPoint : attackTargetCollider.transform.position;

            damagedObject.StartAttack();    
        }
    }

    public void OnEndAttackAnimation()
    {
        if(CharactorFSM.CurrentStateType == CharactorStateType.Attack)
            CharactorFSM.ChangeState(CharactorStateType.Idle);
    }

    public void OnCreateVFX()
    {
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
