using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DamagedObject : MonoBehaviour
{
    [SerializeField]
    protected bool isOwnerFollow;

    [SerializeField]
    protected bool isLifeTime;
    [SerializeField]
    protected float lifeTime;

    [SerializeField]
    protected Transform ownerTransform;

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
    private bool autoDestory;

    [SerializeField]
    protected LayerMask hitLayerMasks;

    protected float currentLifeTime;
    private int currentHitCount;

    protected virtual void Awake()
    {
    }

    protected virtual void Update()
    {
        if (isLifeTime)
        {
            currentLifeTime += Time.deltaTime;

            if (lifeTime < currentLifeTime)
            {
                gameObject.SetActive(false);
                Destroy(this.gameObject);
            }
        }
    }

    protected virtual void FixedUpdate()
    {
        if (isOwnerFollow)
        {
            transform.position = ownerTransform.position;
        }
    }

    protected virtual void AttackHit()
    {
        var hitColliderList = overlapCollider.ColliderList;

        foreach (var hit in hitObjectList)
        {

            if ((hit.gameObject.layer & hitLayerMasks) == 0 || !hitObjectList.Contains(hit.gameObject))
                continue;

            hitObjectList.Add(hit.gameObject);
            hitEvent?.Invoke();
        }
    }

    public virtual void StartAttack()
    {
        startHitEvent?.Invoke();

        AttackHit();
    }

    protected virtual void OnDestroy()
    {
        destoryEvent?.Invoke();
    }

    //public void SetAttackInfo()
    //[SerializeField]
    //private 
}
