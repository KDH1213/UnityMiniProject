using UnityEngine;

[CreateAssetMenu(fileName = "CharactorProfile", menuName = "Profile/CharactorProfile", order = 3)]
[System.Serializable]
public class CharactorProfile : ScriptableObject
{
    [SerializeField]
    private AttackType attackType;
    public AttackType AttackType { get { return attackType; } }

    [SerializeField]
    private float attackpower;
    public float Attackpower { get { return attackpower; } }
    [SerializeField]
    private float masicAttackpower;
    public float MasicAttackpowerv { get { return masicAttackpower; } }

    [SerializeField]
    private float attackRange;
    public float AttackRange { get { return attackRange; } }

    [SerializeField]
    private float attackSpeed;
    public float AttackSpeed { get { return attackSpeed; } }
}
