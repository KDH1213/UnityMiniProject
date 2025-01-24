using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharactorIdleState : CharactorBaseState
{
    private OverlapCollider OverlapCollider;

    [SerializeField] 
    protected LayerMask hitLayerMasks;

    private bool isAttack = true;

    protected override void Awake()
    {
        base.Awake();
        stateType = CharactorStateType.Idle;
    }

    private void Start()
    {
        OverlapCollider = GameObject.FindGameObjectWithTag(Tags.OverlapCollider).GetComponent<OverlapCollider>();
    }

    public override void Enter()
    {

    }

    public override void ExcuteUpdate()
    {
        if(isAttack)
        {
            int count = OverlapCollider.StartOverlapCircle(transform.position, charactorFSM.CharactorData.AttackRange, hitLayerMasks);
            if (count > 0)
            {
                var hitTarget = OverlapCollider.HitColliders;
                int targetIndex = FindeTarget(ref hitTarget, count);

                StartCoroutine(CoAttackReload());
                ((CharactorAttackState)CharactorFSM.StateTable[CharactorStateType.Attack]).SetAttackTarget(hitTarget[targetIndex]);
                CharactorFSM.ChangeState(CharactorStateType.Attack);
            }
        }
    }

    public override void Exit()
    {
        // charactorFSM.Animator.SetBool(DHUtil.MonsterAnimationUtil.hashIsMove, false);
    }

    private IEnumerator CoAttackReload()
    {
        isAttack = false;
        yield return new WaitForSeconds(CharactorFSM.CharactorData.AttackSpeed);
        isAttack = true;
    }

    private int FindeTarget(ref Collider2D[] hitTarget, int count)
    {
        var routeMove = hitTarget[0].GetComponent<IRouteMove>();
        int targetIndex = 0;

        for (int i = 1; i < count; ++i)
        {
            var target = hitTarget[i].GetComponent<IRouteMove>();
            if (routeMove.MoveIndex == target.MoveIndex)
            {
                if (routeMove.MoveDirection.x == 0f)
                {
                    if (Mathf.Abs(routeMove.CurrentPosition.y) < Mathf.Abs(routeMove.CurrentPosition.y))
                    {
                        routeMove = target;
                        targetIndex = i;
                    }
                }
                else
                {
                    if (Mathf.Abs(routeMove.CurrentPosition.x) < Mathf.Abs(routeMove.CurrentPosition.x))
                    {
                        routeMove = target;
                        targetIndex = i;
                    }
                }
            }
            else if ((routeMove.MoveIndex > 0 && target.MoveIndex == 0) || routeMove.MoveIndex < target.MoveIndex)
            {
                routeMove = target;
                targetIndex = i;
            }
        }

        return targetIndex;
    }
}
