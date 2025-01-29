using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharactorIdleState : CharactorBaseState
{
    [SerializeField] 
    protected LayerMask hitLayerMasks;

    private OverlapCollider OverlapCollider;

    private float attackTime = 0f;

    private bool isAttack = true;

    protected override void Awake()
    {
        base.Awake();
        stateType = CharactorStateType.Idle;

        attackTime = 1f / CharactorFSM.CharactorData.AttackSpeed;
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
            int count = OverlapCollider.StartOverlapCircle(transform.position, charactorFSM.CharactorData.AttackRange * 0.5f, hitLayerMasks);
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
        yield return new WaitForSeconds(attackTime);
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
                    if((routeMove.MoveDirection.y > 0f && routeMove.CurrentPosition.y < routeMove.CurrentPosition.y)
                        || routeMove.MoveDirection.y < 0f && routeMove.CurrentPosition.y > routeMove.CurrentPosition.y)
                    {
                        routeMove = target;
                        targetIndex = i;
                    }
                }
                else
                {
                    if ((routeMove.MoveDirection.x > 0f && routeMove.CurrentPosition.x < routeMove.CurrentPosition.x)
                          || routeMove.MoveDirection.x < 0f && routeMove.CurrentPosition.x > routeMove.CurrentPosition.x)
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


    public void OnChangeStatus()
    {
        // TODO :: player Status 제작에 따라 값을 받아게 변경
        // 캐릭터 공격력, 패시브 스킬 적용 여부 미정인 상태
        attackTime = 1f / CharactorFSM.CharactorData.AttackSpeed;
    }
}
