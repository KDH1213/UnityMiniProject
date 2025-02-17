using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Pool;

public class DamagedObject : MonoBehaviour
{
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

    public float Damage {  get; set; }

    [SerializeField] 
    protected bool autoDestory;

    // private AttackInfoData AttackInfoData;

    [SerializeField] 
    protected AttackData attackData = new AttackData();
    public IObjectPool<DamagedObject> DamagedObjectPool { get; private set; }

    [SerializeField] 
    protected float time = 0.5f;
    protected float currentTime = 0f;

    DamageInfo damageInfo = new DamageInfo();

    private void Awake()
    {
        overlapCollider = GameObject.FindWithTag(Tags.OverlapCollider).GetComponent<OverlapCollider>();
    }

    protected virtual void AttackHit()
    {
        int count = overlapCollider.StartOverlapCircle(transform.position, attackData.RealAttackRange * 0.5f, hitLayerMasks);

        if (count == 0)
            return;

        var hitColliders = overlapCollider.HitColliderList;
        GameObject hitObject;

        damageInfo.damage = Damage;
        damageInfo.debuffType = attackData.DebuffType;
        damageInfo.debuffTime = attackData.DebuffTime;
        damageInfo.debuffProbability = attackData.DebuffProbability;
        damageInfo.vfxID = attackData.VFXId;

        for (int i = 0; i < count; ++i)
        {
            hitObject = hitColliders[i].gameObject;

            if (((1 << hitObject.layer) & hitLayerMasks) == 0)
                continue;

            var damageable = hitObject.GetComponent<IDamageable>();
            
            if(damageable == null)
                continue;

            hitObjectList.Add(hitObject);
      
            damageable.OnDamage(ref damageInfo);
            hitEvent?.Invoke();
        }
    }

    public virtual void StartAttack()
    {
        startHitEvent?.Invoke();

        AttackHit();

        Release();
    }

    protected virtual void OnDestroy()
    {
        destoryEvent?.Invoke();
    }

    public void SetAttackData(AttackData data)
    {
        attackData = data;
    }

    public void SetPool(IObjectPool<DamagedObject> damagedObjectObjectPool)
    {
        DamagedObjectPool = damagedObjectObjectPool;
    }

    public void Release()
    {
        destoryEvent?.Invoke();
        DamagedObjectPool.Release(this);
    }
}
