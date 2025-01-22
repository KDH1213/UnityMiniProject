using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DamagedObject : MonoBehaviour
{
    [SerializeField] protected OverlapCollider overlapCollider;
    [SerializeField] protected UnityEvent hitEvent;
    [SerializeField] protected UnityEvent startHitEvent;
    [SerializeField] protected UnityEvent destoryEvent;
    [SerializeField] protected List<GameObject> hitObjectList;
    [SerializeField] private bool autoDestory;
    [SerializeField] protected LayerMask hitLayerMasks;

    // private AttackInfoData AttackInfoData;

    [SerializeField] private AttackData attackData = new AttackData();

    [SerializeField] private float time = 0.5f;
    private float currentTime = 0f;

    private void Start()
    {
        StartAttack();
    }

    private void Update()
    {
        currentTime += Time.deltaTime;

        if (currentTime >= time)
        {
            StartAttack();
            currentTime = 0f;
        }
    }

    protected virtual void AttackHit()
    {
        int count = overlapCollider.StartOverlapCircle(attackData.AttackRange);

        if (count == 0)
            return;

        var hitColliders = overlapCollider.HitColliders;
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
}
