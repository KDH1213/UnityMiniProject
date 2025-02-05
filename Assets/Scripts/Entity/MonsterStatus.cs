using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using AYellowpaper.SerializedCollections;

public class MonsterStatus : MonoBehaviour, IDamageable
{
    [SerializedDictionary, SerializeField]
    private SerializedDictionary<StatType, StatusValue> currentValues = new SerializedDictionary<StatType, StatusValue>();
    public SerializedDictionary<StatType, StatusValue> CurrentValueTable { get {  return currentValues; } }

    [SerializeField] 
    private MonsterProfile monsterProfile;
    [SerializeField] 
    private MonsterFSMController monsterFSMController;

    [SerializeField] 
    private Slider hpbar;
    public bool IsDead { get; private set; } = false;

    public UnityEvent hitEvent;
    public UnityEvent deathEvent;
    public UnityEvent damegedEvent;
    public UnityEvent DeathEvent { get { return deathEvent; } }
    public UnityEvent<float> debuffEvent;

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

        hpbar.value = 1f;
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
        hpbar.gameObject.SetActive(true);

        var currentHp = currentValues[StatType.HP].AddValue(-damage);
        hpbar.value = currentHp / currentValues[StatType.HP].MaxValue;

        hitEvent?.Invoke();

        if (currentHp <= 0)
        {
            IsDead = true;
            inoutDamageInfo.targetDeath = IsDead;

            hpbar.gameObject.SetActive(false);
            deathEvent?.Invoke();
            // Destroy(gameObject);
        }
        else if (inoutDamageInfo.debuffType != DebuffType.None 
            && (inoutDamageInfo.debuffProbability >= 100f || (Random.value * 100f) <= inoutDamageInfo.debuffProbability))
        {
            debuffEvent?.Invoke(inoutDamageInfo.debuffTime);
        }
        else
            damegedEvent?.Invoke();

        return true;
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
}
