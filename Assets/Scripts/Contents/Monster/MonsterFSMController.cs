using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class MonsterFSMController : FSMController<MonsterStateType>
{
    [SerializeField]
    protected Animator animator;
    public Animator Animator { get { return animator; } }

    [SerializeField]
    private MonsterStatus monsterStatus;

    private MonsterSpawner monsterSpawner;
    public MonsterSpawner MonsterSpawner { get { return monsterSpawner; } }

    [SerializeField] 
    private SpriteRenderer[] spriteRenderers;
    public SpriteRenderer[] SpriteRenderers { get { return spriteRenderers; } }

    [SerializeField] 
    private Color hitEffectColor;
    [SerializeField] 
    private float hitEffectTime;
    private Coroutine hitEffectCoroutine;

    protected override void Awake()
    {
        monsterStatus.deathEvent.AddListener(() => ChangeState(MonsterStateType.Death));
        monsterStatus.debuffEvent.AddListener((time)
            => ((MonsterStunState)StateTable[MonsterStateType.Stun]).SetStunTime(time));

        monsterStatus.damegedEvent.AddListener( () =>
            {
                if (hitEffectCoroutine != null)
                    StopCoroutine(hitEffectCoroutine);
                hitEffectCoroutine = StartCoroutine(CoHitEffect());
            });
    }

    protected void Start()
    {
        StartState();
    }

    public virtual void ExcuteUpdate()
    {
        stateTable[currentStateType]?.ExecuteUpdate();
    }

    public virtual void ExcuteFixedUpdate()
    {
        stateTable[currentStateType]?.ExecuteFixedUpdate();
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

    private IEnumerator CoHitEffect()
    {
        var originalColor = spriteRenderers[0].color;

        foreach (var sprite in spriteRenderers)
        {
            sprite.color = hitEffectColor;
        }

        yield return new WaitForSeconds(hitEffectTime);

        if(currentStateType == MonsterStateType.Move)
        {
            foreach (var sprite in spriteRenderers)
            {
                sprite.color = originalColor;
            }
        }     
    }
}