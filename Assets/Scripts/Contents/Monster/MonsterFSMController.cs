using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Pool;

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

    public UnityEvent<MonsterFSMController> destoryEvent;

    public IObjectPool<MonsterFSMController> MonsterPool { get; private set; }

    private Color originColor;

    private UniTask uniTaskHitEffect;
    private CancellationTokenSource hitEffectCoroutineSource = new();

    protected override void Awake()
    {
        originColor = spriteRenderers[0].color;

        monsterStatus.deathEvent.AddListener(() => ChangeState(MonsterStateType.Death));
        monsterStatus.debuffEvent.AddListener((debuffType, time)
            => { if (debuffType == DebuffType.Stun) ((MonsterStunState)StateTable[MonsterStateType.Stun]).SetStunTime(time); });

        monsterStatus.damegedEvent.AddListener(() =>
            {
                if (uniTaskHitEffect.Status == UniTaskStatus.Pending)
                {
                    hitEffectCoroutineSource.Cancel();
                }
                uniTaskHitEffect = UniHitEffect();
                //if (hitEffectCoroutine != null)
                //    StopCoroutine(hitEffectCoroutine);
                //hitEffectCoroutine = StartCoroutine(CoHitEffect());
            });
    }

    protected void Start()
    {
        StartState();

        //var spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
       
        //foreach (var spriteRenderer in spriteRenderers)
        //{
        //    var material = spriteRenderer.material;
        //    material.color = hitEffectColor;
        //    // spriteRenderers[0].material = material;
        //    // var material = spriteRenderers[0].material;
        //    // material.color = hitEffectColor;
        //}
       
    }

    private void OnDestroy()
    {
        hitEffectCoroutineSource.Cancel();
        hitEffectCoroutineSource.Dispose();
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

    public void SetPool(IObjectPool<MonsterFSMController> objectPool)
    {
        MonsterPool = objectPool;
    }

    public void Release()
    {
        destoryEvent?.Invoke(this);
        MonsterPool.Release(this);
    }

    public void SetSpriteRenderer()
    {
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
    }
    private async UniTask UniHitEffect()
    {
        var originalColor = originColor;

        foreach (var sprite in spriteRenderers)
        {
            sprite.color = hitEffectColor;
        }
        await UniTask.Delay(TimeSpan.FromSeconds(hitEffectTime), cancellationToken:this.destroyCancellationToken);

        if (currentStateType == MonsterStateType.Move || currentStateType == MonsterStateType.Stun)
        {
            foreach (var sprite in spriteRenderers)
            {
                sprite.color = originalColor;
            }
        }
    }
    //private IEnumerator CoHitEffect()
    //{
    //    var originalColor = originColor;

    //    foreach (var sprite in spriteRenderers)
    //    {
    //        sprite.color = hitEffectColor;
    //    }

    //    yield return new WaitForSeconds(hitEffectTime);

    //    if(currentStateType == MonsterStateType.Move)
    //    {
    //        foreach (var sprite in spriteRenderers)
    //        {
    //            sprite.color = originalColor;
    //        }
    //    }     
    //}
}