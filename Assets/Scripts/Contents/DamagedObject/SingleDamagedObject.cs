using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleDamagedObject : DamagedObject
{
    public GameObject attackTarget { get; set; }

    //protected override void Start()
    //{
    //    if (autoDestory)
    //    {
    //        Destroy(gameObject, time);
    //    }
    //}

    protected override void AttackHit()
    {
        var damageable = attackTarget.GetComponent<IDamageable>();
        DamageInfo damageInfo = new DamageInfo();
        damageInfo.damage = Damage;
        damageInfo.debuffType = attackData.DebuffType;
        damageInfo.debuffProbability = attackData.DebuffProbability;
        damageInfo.vfxID = attackData.VFXId;

        damageable.OnDamage(ref damageInfo);
        hitEvent?.Invoke();
    }
}
