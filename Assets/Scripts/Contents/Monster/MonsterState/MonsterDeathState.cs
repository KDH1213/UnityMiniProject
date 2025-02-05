using System.Collections;
using UnityEngine;

public class MonsterDeathState : MonsterBaseState
{
    [SerializeField]
    private Collider2D monsterCollider;

    [SerializeField] 
    private float deathEffectTime;
    protected override void Awake()
    {
        base.Awake();
        stateType = MonsterStateType.Death;
        monsterCollider = GetComponent<Collider2D>();
    }

    public override void Enter()
    {
        enterStateEvent?.Invoke();
        StartCoroutine(CoDeathEffectTime());
        monsterFSM.Animator.SetTrigger(DHUtil.MonsterAnimationUtil.hashIsDeath);
        monsterCollider.enabled = false;
    }
    public override void ExecuteUpdate()
    {
    }
    public override void ExecuteFixedUpdate()
    {
    }

    public override void Exit()
    {
        exitStateEvent?.Invoke();
    }

    private IEnumerator CoDeathEffectTime()
    {
        var spriteRenderers = MonsterFSM.GetComponentsInChildren<SpriteRenderer>();
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

        Destroy(gameObject);
    }
}
