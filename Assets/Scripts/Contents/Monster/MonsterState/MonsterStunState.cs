using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cysharp.Threading.Tasks;
using System.Threading;

public class MonsterStunState : MonsterBaseState
{
    [SerializeField] 
    private Color stunColor;
    private float stunTime;
    private Coroutine stunCoroutine;

    private float currentStunTime = 0f;

    private UniTask stunTimeTask;
    private CancellationTokenSource stunCoroutineSource = new();

    protected override void Awake()
    {
        base.Awake();
        stateType = MonsterStateType.Stun;
    }

    private void OnDisable()
    {
        //if(stunCoroutine != null)
        //{
        //    StopCoroutine(stunCoroutine);
        //    stunCoroutine = null;
        //}

        stunCoroutineSource.Cancel();
    }

    public override void Enter()
    {
        enterStateEvent?.Invoke();

        //if(stunCoroutine != null)
        //{
        //    StopCoroutine(stunCoroutine);
        //}

        //stunCoroutine = StartCoroutine(CoStunTime());

        if(stunTimeTask.Status == UniTaskStatus.Pending)
        {
            stunCoroutineSource.Cancel(true);
        }
        stunTimeTask = UniTaskStunTime();


        monsterFSM.Animator.SetBool(DHUtil.MonsterAnimationUtil.hashIsDebuff, true);
    }
    public override void ExecuteUpdate()
    {
    }
    public override void ExecuteFixedUpdate()
    {
    }

    public override void Exit()
    {
        stunCoroutineSource.Cancel();
        exitStateEvent?.Invoke();
        monsterFSM.Animator.SetBool(DHUtil.MonsterAnimationUtil.hashIsDebuff, false);
    }

    public void SetStunTime(float time)
    {
        if (currentStunTime > time)
            return;

        stunTime = time;
        MonsterFSM.ChangeState(MonsterStateType.Stun);
    }

    private async UniTask UniTaskStunTime()
    {
        currentStunTime = stunTime;

        while (currentStunTime > 0f)
        {
            await UniTask.Yield(PlayerLoopTiming.Update);
            currentStunTime -= Time.deltaTime;
        }

        if (MonsterFSM.CurrentStateType != MonsterStateType.Death)
            MonsterFSM.ChangeState(MonsterStateType.Move);
    }
    private IEnumerator CoStunTime()
    {
        //var spriteRenderers = MonsterFSM.SpriteRenderers;
        //var originalColor = spriteRenderers[0].color;

        //foreach (var sprite in spriteRenderers)
        //{
        //    sprite.color = stunColor;
        //}

        currentStunTime = stunTime;

        while (currentStunTime > 0f)
        {
            yield return new WaitForEndOfFrame();
            currentStunTime -= Time.deltaTime;
        }

        //foreach (var sprite in spriteRenderers)
        //{
        //    sprite.color = originalColor;
        //}

        if(MonsterFSM.CurrentStateType != MonsterStateType.Death)
            MonsterFSM.ChangeState(MonsterStateType.Move);
    }

    private void OnDestroy()
    {
        stunCoroutineSource.Cancel();
        stunCoroutineSource.Dispose();
    }
}
