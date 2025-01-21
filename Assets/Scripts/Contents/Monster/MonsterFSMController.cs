using UnityEngine;
using UnityEngine.Events;

public class MonsterFSMController : FSMController<MonsterStateType>
{
    [SerializeField] protected Animator animator;
    public Animator Animator {  get { return animator; } }

    [SerializeField] private MonsterStatus monsterStatus;

    private MonsterSpawner monsterSpawner;
    public MonsterSpawner MonsterSpawner { get { return monsterSpawner; } }
    [SerializeField] private SpriteRenderer[] spriteRenderers;
    public SpriteRenderer[] SpriteRenderers { get { return spriteRenderers; } }

    private bool isMove = false;
    private bool isDestination = false;

    protected override void Awake()
    {
        monsterStatus.deathEvent.AddListener(() => ChangeState(MonsterStateType.Death));
        monsterStatus.debuffEvent.AddListener((time)
            => ((MonsterStunState)StateTable[MonsterStateType.Stun]).SetStunTime(1f));
    }

    protected void Start()
    {
        StartState();
    }

    private void Update()
    {
        ExcuteUpdate();
    }

    protected virtual void ExcuteUpdate()
    {
        stateTable[currentStateType]?.ExcuteUpdate();
    }

    protected virtual void ExcuteFixedUpdate()
    {
        stateTable[currentStateType]?.ExcuteFixedUpdate();
    }

    public void OnSpawn(MonsterSpawner monsterSpawner)
    {
        this.monsterSpawner = monsterSpawner;

        // isMove = monsterSpawner.GetMovePoint(ref movePoint, ref moveDirection, ref moveIndex);
    }

    public float GetStatValue(StatType statType)
    {
        return monsterStatus.GetStatValue(statType);
    }
}