using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterHitState : MonsterBaseState
{
    protected override void Awake()
    {
        base.Awake();
        stateType = MonsterStateType.Hit;
    }
}
