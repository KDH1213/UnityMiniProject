using UnityEngine;

[CreateAssetMenu(fileName = "EnemyProfile", menuName = "Profile/EnemyProfile", order = 3)]
[System.Serializable]
public class MonsterProfile : ScriptableObject
{
    [SerializeField]
    private StatusInfoData enemyStatusInfoData;
    [SerializeField]
    public StatusInfoData EnemyStatusInfoData { get { return enemyStatusInfoData; } }

    [SerializeField]
    private int money;
    public int Money { get { return money; } }
}
