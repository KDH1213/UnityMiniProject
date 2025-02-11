using Cysharp.Threading.Tasks;
using System.Collections;
using System.Threading;
using UnityEngine;

public class MonsterDeathState : MonsterBaseState
{
    private Collider2D monsterCollider;

    [SerializeField] 
    private float deathEffectTime;

    private SpriteRenderer monsterSprite;
    private Color currentColor;

    private UniTask uniTaskDeathEffectTime;

    private float currentTime = 0f;
   

    protected override void Awake()
    {
        base.Awake();
        stateType = MonsterStateType.Death;
        monsterCollider = GetComponent<Collider2D>();
    }

    private void OnDisable()
    {
        monsterCollider.enabled = true;
    }

    public override void Enter()
    {
        enterStateEvent?.Invoke();
        // StartCoroutine(CoDeathEffectTime());
        // uniTaskDeathEffectTime = UniTaskDeathEffectTime(); 
        currentTime = 0f;

        monsterFSM.Animator.SetTrigger(DHUtil.MonsterAnimationUtil.hashIsDeath);
        monsterCollider.enabled = false;

        monsterSprite = MonsterFSM.SpriteRenderers[0];
    }
    public override void ExecuteUpdate()
    {
        currentColor = monsterSprite.color;
        currentTime += Time.deltaTime;
        currentColor.a = (deathEffectTime - currentTime) / deathEffectTime;

        foreach (var sprite in MonsterFSM.SpriteRenderers)
        {
            sprite.color = currentColor;
        }

        if (currentTime > deathEffectTime)
        {
            monsterFSM.Release();

            foreach (var sprite in MonsterFSM.SpriteRenderers)
            {
                sprite.color = Color.white;
            }
        }
    }
    public override void ExecuteFixedUpdate()
    {
    }

    public override void Exit()
    {
        exitStateEvent?.Invoke();
    }

    private async UniTask UniTaskDeathEffectTime()
    {
        var spriteRenderers = MonsterFSM.SpriteRenderers;
        var currentColor = spriteRenderers[0].color;
        float currentTime = 0f;
        while (currentTime < deathEffectTime)
        {
            currentTime += Time.deltaTime;
            currentColor.a = (deathEffectTime - currentTime) / deathEffectTime;

            foreach (var sprite in spriteRenderers)
            {
                sprite.color = currentColor;
            }

            await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken: this.GetCancellationTokenOnDestroy());
        }

        monsterFSM.Release();

        foreach (var sprite in spriteRenderers)
        {
            sprite.color = Color.white;
        }
    }

    private IEnumerator CoDeathEffectTime()
    {
        var spriteRenderers = MonsterFSM.SpriteRenderers;
        var currentColor = spriteRenderers[0].color;
        float currentTime = 0f;
        while (currentTime < deathEffectTime)
        {
            currentTime += Time.deltaTime;
            currentColor.a = (deathEffectTime - currentTime) / deathEffectTime;

            foreach (var sprite in spriteRenderers)
            {
                sprite.color = currentColor;
            }

            yield return new WaitForEndOfFrame();
        }

        monsterFSM.Release();

        foreach (var sprite in spriteRenderers)
        {
            sprite.color = Color.white;
        }
    }
}
