using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterHitState : MonsterBaseState
{
    protected void Awake()
    {
        stateType = MonsterStateType.Hit;
    }
}
