using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class MonsterStunState : MonsterBaseState
{
    [SerializeField] private Color stunColor;
    private float stunTime;
    private Coroutine stunCoroutine;

    protected override void Awake()
    {
        base.Awake();
        stateType = MonsterStateType.Stun;
    }

    public override void Enter()
    {
        enterStateEvent?.Invoke();

        if(stunCoroutine != null)
        {
            StopCoroutine(stunCoroutine);
        }

        stunCoroutine = StartCoroutine(CoStunTime());
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

    public void SetStunTime(float time)
    {
        stunTime = time;
        MonsterFSM.ChangeState(MonsterStateType.Stun);
    }

    private IEnumerator CoStunTime()
    {
        var spriteRenderers = MonsterFSM.SpriteRenderers;
        var originalColor = spriteRenderers[0].color;

        foreach (var sprite in spriteRenderers)
        {
            sprite.color = stunColor;
        }

        yield return new WaitForSeconds(stunTime);

        foreach (var sprite in spriteRenderers)
        {
            sprite.color = originalColor;
        }

        if(MonsterFSM.CurrentStateType != MonsterStateType.Death)
            MonsterFSM.ChangeState(MonsterStateType.Move);
    }
}
