using UnityEngine;

[CreateAssetMenu(fileName = "EnemyProfile", menuName = "Profile/EnemyProfile", order = 3)]
[System.Serializable]
public class EnemyProfile : ScriptableObject
{
    [SerializeField]
    private MoveType moveType;
    public MoveType MoveType { get { return moveType; } }

    [SerializeField]
    private StatusInfoData enemyStatusInfoData;
    [SerializeField]
    public StatusInfoData EnemyStatusInfoData { get { return enemyStatusInfoData; } }

    [SerializeField]
    private int money;
    public int Money { get { return money; } }
}
