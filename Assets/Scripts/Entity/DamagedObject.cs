using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DamagedObject : MonoBehaviour
{
    [SerializeField] 
    protected OverlapCollider overlapCollider;

    [SerializeField] 
    protected UnityEvent hitEvent;
    [SerializeField] 
    protected UnityEvent startHitEvent;
    [SerializeField] 
    protected UnityEvent destoryEvent;

    [SerializeField] 
    protected List<GameObject> hitObjectList;

    [SerializeField] 
    protected LayerMask hitLayerMasks;

    [SerializeField] 
    protected bool autoDestory;

    // private AttackInfoData AttackInfoData;

    [SerializeField] 
    protected AttackData attackData = new AttackData();

    [SerializeField] 
    protected float time = 0.5f;
    protected float currentTime = 0f;

    protected virtual void Start()
    {
        StartAttack();

        if(autoDestory)
            Destroy(gameObject, time);
    }

    //private void Update()
    //{
    //    currentTime += Time.deltaTime;

    //    if (currentTime >= time)
    //    {
    //        StartAttack();
    //        currentTime = 0f;
    //    }
    //}

    protected virtual void AttackHit()
    {
        int count = overlapCollider.StartOverlapCircle(attackData.AttackRange * 0.5f);

        if (count == 0)
            return;

        var hitColliders = overlapCollider.HitColliderList;
        GameObject? hitObject;

        for (int i = 0; i < count; ++i)
        {
            hitObject = hitColliders[i].gameObject;

            if (((1 << hitObject.layer) & hitLayerMasks) == 0 || hitObjectList.Contains(hitObject))
                continue;

            var damageable = hitObject.GetComponent<IDamageable>();
            
            if(damageable == null)
                continue;

            hitObjectList.Add(hitObject);
            DamageInfo damageInfo = new DamageInfo();
            damageInfo.damage = attackData.Damage;
            damageInfo.debuffType = attackData.DebuffType;
            damageInfo.debuffTime = attackData.DebuffTime;
            damageInfo.debuffProbability = attackData.DebuffProbability;

            damageable.OnDamage(ref damageInfo);
            hitEvent?.Invoke();
        }


        //foreach (var hit in hitObjectList)
        //{
        //    if ((hit.gameObject.layer & hitLayerMasks) == 0 || !hitObjectList.Contains(hit.gameObject))
        //        continue;

        //    hitObjectList.Add(hit.gameObject);
        //    // hit.GetComponent<IDamageable>().OnDamage()
        //    hitEvent?.Invoke();
        //}
    }

    public virtual void StartAttack()
    {
        startHitEvent?.Invoke();

        AttackHit();

        //if(autoDestory)
        //{
        //    Destroy(gameObject);
        //}
    }

    protected virtual void OnDestroy()
    {
        destoryEvent?.Invoke();
    }

    public void SetAttackData(AttackData data)
    {
        attackData = data;
    }

    private void OnDrawGizmos()
    {
    }
}
