using UnityEngine;
using UnityEngine.Events;

public class MonsterController : MonoBehaviour
{
    [SerializeField] private MonsterStatus enemyStatus;

    [SerializeField] private Vector2 movePoint;
    private Vector2 moveDirection;
    private Vector2 currentPosition;

    private MonsterSpawner monsterSpawner;

    [SerializeField] private float moveSpeed;
    private int moveIndex = 0;
    private bool isMove = false;
    private bool isDestination = false;

    private void Start()
    {
        // moveSpeed = enemyStatus.GetStatValue(StatType.MovementSpeed);
        currentPosition = transform.position;
    }

    private void Update()
    {
        Move();
    }

    public void OnSpawn(MonsterSpawner monsterSpawner)
    {
        this.monsterSpawner = monsterSpawner;
        isMove = monsterSpawner.GetMovePoint(ref movePoint, ref moveDirection, ref moveIndex);
    }

    public void Move()
    {
        currentPosition += moveDirection * (moveSpeed * Time.deltaTime);
        transform.position = (Vector3)currentPosition;

        if ((currentPosition - movePoint).sqrMagnitude < 1f)
        {
            ++moveIndex;
            isMove = monsterSpawner.GetMovePoint(ref movePoint, ref moveDirection, ref moveIndex);
        }
    }
}