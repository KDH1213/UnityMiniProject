using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using AYellowpaper.SerializedCollections;
using UnityEngine.Pool;

public class MonsterStatus : MonoBehaviour, IDamageable
{
    [SerializedDictionary, SerializeField]
    private SerializedDictionary<StatType, StatusValue> currentValues = new SerializedDictionary<StatType, StatusValue>();
    public SerializedDictionary<StatType, StatusValue> CurrentValueTable { get {  return currentValues; } }

    [SerializeField]
    private VfxContainerData vfxContainerData;
    [SerializeField] 
    private MonsterFSMController monsterFSMController;
    [SerializeField]
    private Transform monseterVFXHitPoint;
    
    public bool IsDead { get; private set; } = false;

    public UnityEvent hitEvent;
    public UnityEvent deathEvent;
    public UnityEvent damegedEvent;
    public UnityEvent DeathEvent { get { return deathEvent; } }
    public UnityEvent<DebuffType, float> debuffEvent;

    public UnityAction<int> CoinQtyAction;
    public UnityAction<int> JewelQtyAction;
    public UnityEvent<float> onChangeHpEvnet;

    private IObjectPool<UIDamageText> uIDamageObjectTextPool;
    private IObjectPool<UIDamageTextMeshRenderer> uIDamageObjectTextMeshRendererPool;
    private VFXObjectPool vFXObjectPool;

    private void Awake()
    {
        //foreach (var item in enemyProfile.EnemyStatusInfoData.StatusTable)
        //{
        //    if (!currentValues.ContainsKey(item.Key))
        //    {
        //        StatusValue status = new StatusValue(item.Key);
        //        currentValues.Add(item.Key, status);
        //    }

        //    currentValues[item.Key].ValueCopy(item.Value);
        //}

    }

    private void OnEnable()
    {
        IsDead = false;
    }

    public bool OnDamage(ref DamageInfo inoutDamageInfo)
    {
        if (IsDead)
            return false;

        var damage = DamageCalculate(ref inoutDamageInfo);

        if (damage == 0f)
        {
            inoutDamageInfo.targetDeath = false;
            return false;
        }

        var currentHp = currentValues[StatType.HP].AddValue(-damage);
        //var damageText = uIDamageObjectTextMeshRendererPool.Get();

        //damageText.transform.position = transform.position;
        //damageText.SetDamage(((int)damage));

        // Instantiate(uIDamageTextPrefab, transform.position, Quaternion.identity).SetDamage(damage.ToString());

        //if (vfxContainerData.VfxContainerTable.ContainsKey(inoutDamageInfo.vfxID))
        //{
        //    foreach (var vfx in vfxContainerData.VfxContainerTable[inoutDamageInfo.vfxID])
        //    {
        //        Instantiate(vfx, transform.position, Quaternion.identity);
        //    }
        //}

        if (inoutDamageInfo.vfxID != 0)
        {
            var createTransform = vFXObjectPool.GetVFX(inoutDamageInfo.vfxID).transform;

            var followTarget = createTransform.GetComponent<FollowTarget>();

            if (followTarget != null)
            {
                followTarget.SetTarget(transform);
            }
            else
            {
                createTransform.SetParent(monseterVFXHitPoint);
                createTransform.localPosition = Vector3.zero;
            }
        }

        hitEvent?.Invoke();

        if (currentHp <= 0)
        {
            IsDead = true;
            inoutDamageInfo.targetDeath = IsDead;

            deathEvent?.Invoke();

            CoinQtyAction.Invoke((int)currentValues[StatType.CoinQty].Value);
            JewelQtyAction.Invoke((int)currentValues[StatType.JewelQty].Value);
            // Destroy(gameObject);
        }
        else if (inoutDamageInfo.debuffType != DebuffType.None 
            && (inoutDamageInfo.debuffProbability >= 100f || (Random.value * 100f) <= inoutDamageInfo.debuffProbability))
        {
            debuffEvent?.Invoke(inoutDamageInfo.debuffType, inoutDamageInfo.debuffTime);
        }
        else
            damegedEvent?.Invoke();

        return true;
    }
    public void OnChangeHp() //*HP °»½Å
    {
        var hpValue = currentValues[StatType.HP];

        onChangeHpEvnet?.Invoke(hpValue.PersentValue);

        if (hpValue.Value <= 0f)
        {
            IsDead = true;
        }
    }
    private float DamageCalculate(ref DamageInfo inoutDamageInfo)
    {
        float damage = inoutDamageInfo.damage;// - currentValues[StatType.Defense].Value;

        return (damage <= 0f ? 0f : damage);
    }

    public float GetStatValue(StatType statType)
    {
        return currentValues[statType].Value;
    }

    public void SetUIDamageObjectTextPool(UIDamageObjectTextPool uIDamageObjectTextPool)
    {
        this.uIDamageObjectTextMeshRendererPool = uIDamageObjectTextPool.UiDamageTextMeshRendererPool;
    }

    public void SetVFXObjectPool(VFXObjectPool vFXObjectPool)
    {
        this.vFXObjectPool = vFXObjectPool;
    }
}
