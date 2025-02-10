using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UICharactorInfo : MonoBehaviour
{
    [SerializeField]
    private ReinforcedManager reinforcedManager;
    [SerializeField]
    private Image charactorImage;
    [SerializeField]
    private TextMeshProUGUI charactorType;
    [SerializeField]
    private TextMeshProUGUI charactorName;

    [SerializeField]
    private Image attackImage;
    [SerializeField]
    private Image attackSpeedImage;

    [SerializeField]
    private TextMeshProUGUI attackDamage;

    [SerializeField]
    private TextMeshProUGUI reinforcedAttackDamage;
    [SerializeField]
    private TextMeshProUGUI attackSpeed;
    [SerializeField]
    private TextMeshProUGUI attackName;
    [SerializeField]
    private TextMeshProUGUI attackType;

    [SerializeField]
    private TextMeshProUGUI debuffName;
    [SerializeField]
    private TextMeshProUGUI debuffTypeName;
    [SerializeField]
    private TextMeshProUGUI debuffProbability;
    [SerializeField]
    private TextMeshProUGUI debuffTime;

    public CharactorData CharactorData { get; private set; }
    public AttackData AttackData { get; private set; }

    private readonly string charactorTypeFormat = "{0}";
    private readonly string charactorNameFormat = "{0}";

    private readonly string attackNameFormat = "{0}";
    private readonly string attackTypeFormat = "{0}";

    private readonly string damageFormat = "{0}";
    private readonly string reinforcedDamageFormat = "+ {0:f2}";
    private readonly string attackSpeedFormat = "{0:f1}";


    private readonly string debuffNameFormat = "{0}";
    private readonly string debuffTypeFormat = "{0}";
    private readonly string debuffProbabilityFormat = "{0:f2}%";
    private readonly string debuffTimeFormat = "{0:f2}초";


    private void Awake()
    {
        DefaultSetting();
        SetEmpty();
    }

    public void SetEmpty()
    {
        CharactorData = null;
        AttackData = null;
    }

    public void SetData(CharactorData charactorData)
    {
        CharactorData = charactorData;
        AttackData = DataTableManager.AttackDataTable.Get(CharactorData.AttackInfoID);
        var level = reinforcedManager.GetReinforcedLevel(CharactorData.CharactorClassType);

        charactorType.text = string.Format(charactorTypeFormat, CharactorData.CharactorClassType.ToString());
        charactorName.text = string.Format(charactorNameFormat, CharactorData.PrefabID);
        attackDamage.text = string.Format(damageFormat, CharactorData.Damage.ToString());
        attackSpeed.text = string.Format(attackSpeedFormat, CharactorData.AttackSpeed.ToString());

        attackType.text = string.Format(attackTypeFormat, TypeStringTable.AttackTypeStrings[(int)AttackData.AttackType]);
        debuffTypeName.text = string.Format(debuffTypeFormat, TypeStringTable.DebuffTypeStrings[(int)AttackData.DebuffType]);

        charactorImage.sprite = CharactorData.Icon;

        if (level == 0)
        {
            reinforcedAttackDamage.gameObject.SetActive(false);
        }
        else
        {
            reinforcedAttackDamage.gameObject.SetActive(true);
            var extraDamage = CharactorData.Damage * (reinforcedManager.GetCurrentReinforcedDamagePercent(CharactorData.CharactorClassType) * 0.01f);
            reinforcedAttackDamage.text = string.Format(reinforcedDamageFormat, extraDamage);
        }

        if(AttackData.DebuffType != DebuffType.None)
        {
            debuffProbability.text = string.Format(debuffProbabilityFormat, AttackData.DebuffProbability.ToString());
            debuffTime.text = string.Format(debuffTimeFormat, AttackData.DebuffTime.ToString());
        }
        else
        {
            debuffProbability.text = string.Empty;
            debuffTime.text = string.Empty;
        }
    }

    private void DefaultSetting()
    {
        // attackImage;
        // attackSpeedImage;

        attackName.text = string.Format(attackNameFormat, "공격 타입");
        debuffName.text = string.Format(debuffNameFormat, "디버프 타입"); 
    }
}
