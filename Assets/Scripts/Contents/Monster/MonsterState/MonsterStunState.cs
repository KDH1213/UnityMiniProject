using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStunState : MonsterBaseState
{
    private float stunTime;
    private Coroutine stunCoroutine;
    [SerializeField] private SpriteRenderer spriteRenderer;
    private Color originalColor;

    protected override void Awake()
    {
        base.Awake();
        stateType = MonsterStateType.Stun;
        originalColor = spriteRenderer.color;
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
        var effectColor = originalColor;
        effectColor.a = 0.5f;
        spriteRenderer.color = effectColor;

         yield return stunTime;

        spriteRenderer.color = originalColor;
        MonsterFSM.ChangeState(MonsterStateType.Move);
    }
}
