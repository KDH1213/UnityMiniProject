using System.Collections;
using UnityEngine;

public class MonsterDeathState : MonsterBaseState
{
    private SpriteRenderer spriteRenderer;
    [SerializeField] private float deathEffectTime;
    protected void Awake()
    {
        stateType = MonsterStateType.Death;
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
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
        float currentTime = 0f;
        var currentColor = spriteRenderer.color;
        while (currentTime <= deathEffectTime)
        {
            currentTime += Time.deltaTime;
            currentColor.a = (deathEffectTime - currentTime) / deathEffectTime;

            yield return new WaitForEndOfFrame();
        }
        Destroy(gameObject);
    }
}
