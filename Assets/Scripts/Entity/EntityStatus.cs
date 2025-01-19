using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class EntityStatus : MonoBehaviour, IDamageable
{
    [SerializeField]
    protected StatusInfoData originalStatusData;

    [SerializeField]
    protected StatusContainer currentStatus;

    public StatusContainer CurrentStatus
    {
        get { return currentStatus; }
    }

    public float GetStatValue(StatType statusInfoType)
    {
        return currentStatus.GetStatusTable[statusInfoType].Value;
    }

    protected bool isDead = false;
    public bool IsDead { get { return isDead; } }

    protected bool isHit = true;
    public bool IsHit { get { return isHit; } }

    public UnityEvent DeathEvent { get { return deathEvent; } }

    public UnityEvent deathEvent;
    public UnityEvent hitEvent;
    public UnityEvent<uint> debuffEvent;

    public UnityAction<float> attackTargetDeathEvent;

    private void Reset()
    {
        CopyStatus();
    }

    private void Awake()
    {
        CopyStatus();
    }

    private void CopyStatus()
    {
        if (originalStatusData != null)
            return;

        currentStatus.CopyStatus(originalStatusData);
    }

    public virtual bool OnDamage(ref DamageInfo damageInfo)
    {
        if (isDead || !isHit)
            return false;

        float currentHP = currentStatus.GetStat(StatType.HP).AddValue(-damageInfo.damage);

        //해당 이벤트는 이펙트 관련 및 피격시 무조건 동작 되어야 하는 기능 모음
        hitEvent?.Invoke();

        if (currentHP <= 0f)
        {
            damageInfo.targetDeath = true;
            // damageInfo.attackOwner.GetComponent<EntityStatus>().attackTargetDeathEvent?.Invoke(currentStatus.GetStatusTable[StatType.Experience].Value);
            deathEvent?.Invoke();
        }
        else
        {
            if (damageInfo.debuffData != null)
            {
                debuffEvent?.Invoke(damageInfo.debuffData.debuffLayer);
            }

            if (damageInfo.knockbackData != null)
            {

            }
        }

        return true;
    }
}
