using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using AYellowpaper.SerializedCollections;

public class MonsterStatus : MonoBehaviour, IDamageable
{
    [SerializedDictionary, SerializeField]
    private SerializedDictionary<StatType, StatusValue> currentValues = new SerializedDictionary<StatType, StatusValue>();

    [SerializeField] private MonsterProfile monsterProfile;
    [SerializeField] private MonsterFSMController monsterFSMController;

    [SerializeField]
    private Image hpbar;
    public bool IsDead { get; private set; } = false;


    public UnityEvent hitEvent;
    public UnityEvent deathEvent;
    public UnityEvent DeathEvent { get { return deathEvent; } }
    public UnityEvent<float> debuffEvent;

    [SerializeField] private float Hp;
    private float currentHp;

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

        currentHp = Hp;
        hpbar.fillAmount = 1f;
    }


    public bool OnDamage(ref DamageInfo inoutDamageInfo)
    {
        var damage = DamageCalculate(ref inoutDamageInfo);

        if (damage == 0f)
        {
            inoutDamageInfo.targetDeath = false;
            return false;
        }

        currentHp -= damage;
        //  currentHp = currentValues[StatType.HP].AddValue(-damage);
        // hpbar.fillAmount = currentHp / currentValues[StatType.HP].MaxValue;
        hpbar.fillAmount = currentHp / Hp;

        hitEvent?.Invoke();

        if (currentHp <= 0)
        {
            IsDead = true;
            inoutDamageInfo.targetDeath = IsDead;
            // GameController.Instance.AddMoney(enemyProfile.Money);
            deathEvent?.Invoke();
            Destroy(gameObject);
        }
        else if (inoutDamageInfo.debuffType != DebuffType.None)
        {
            debuffEvent?.Invoke(inoutDamageInfo.debuffTime);
        }

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
