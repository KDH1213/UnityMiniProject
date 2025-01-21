using System.Collections;
using UnityEngine;

public class MonsterDeathState : MonsterBaseState
{
    [SerializeField] private float deathEffectTime;
    protected override void Awake()
    {
        base.Awake();
        stateType = MonsterStateType.Death;
    }

    public override void Enter()
    {
        enterStateEvent?.Invoke();
        StartCoroutine(CoDeathEffectTime());
        monsterFSM.Animator.SetBool(DHUtil.MonsterAnimationUtil.hashIsMove, false);
    }
    public override void ExcuteUpdate()
    {
    }
    public override void ExcuteFixedUpdate()
    {
    }

    public override void Exit()
    {
        exitStateEvent?.Invoke();
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

        Destroy(gameObject);
    }
}
