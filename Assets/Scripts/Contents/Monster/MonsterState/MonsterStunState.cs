using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cysharp.Threading.Tasks;
using System.Threading;

public class MonsterStunState : MonsterBaseState
{
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
        currentStunTime = stunTime;

        //if(stunCoroutine != null)
        //{
        //    StopCoroutine(stunCoroutine);
        //}

        //stunCoroutine = StartCoroutine(CoStunTime());

        //if(stunTimeTask.Status == UniTaskStatus.Pending)
        //{
        //    stunTimeTask.ToCancellationToken();
        //}
        // stunTimeTask = UniTaskStunTime();

        monsterFSM.Animator.SetBool(DHUtil.MonsterAnimationUtil.hashIsDebuff, true);
    }
    public override void ExecuteUpdate()
    {
        currentStunTime -= Time.deltaTime;

        if (currentStunTime <= 0f)
            MonsterFSM.ChangeState(MonsterStateType.Move);

    }
    public override void ExecuteFixedUpdate()
    {
    }

    public override void Exit()
    {
        //if (stunTimeTask.Status == UniTaskStatus.Pending)
        //{
        //    stunTimeTask.ToCancellationToken();
        //}

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
        currentStunTime = stunTime;

        while (currentStunTime > 0f)
        {
            yield return new WaitForEndOfFrame();
            currentStunTime -= Time.deltaTime;
        }

        if(MonsterFSM.CurrentStateType != MonsterStateType.Death)
            MonsterFSM.ChangeState(MonsterStateType.Move);
    }

    private void OnDestroy()
    {
        stunCoroutineSource.Cancel();
        stunCoroutineSource.Dispose();
    }
}
