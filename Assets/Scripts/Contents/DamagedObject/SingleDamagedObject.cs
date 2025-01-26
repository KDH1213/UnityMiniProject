using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleDamagedObject : DamagedObject
{
    public GameObject attackTarget { get; set; }

    protected override void Start()
    {
        if (autoDestory)
        {
            Destroy(gameObject, time);
        }
    }

    protected override void AttackHit()
    {
        var damageable = attackTarget.GetComponent<IDamageable>();
        DamageInfo damageInfo = new DamageInfo();
        damageInfo.damage = attackData.Damage;
        damageInfo.debuffType = attackData.DebuffType;
        damageInfo.debuffProbability = attackData.DebuffProbability;

        damageable.OnDamage(ref damageInfo);
        hitEvent?.Invoke();
    }

    public override void StartAttack()
    {
        startHitEvent?.Invoke();

        AttackHit();

        //if(autoDestory)
        //{
        //    Destroy(gameObject);
        //}
    }

    protected override void OnDestroy()
    {
        destoryEvent?.Invoke();
    }
}
