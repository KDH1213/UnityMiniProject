[System.Serializable]

public enum StateType 
{
    None,
    Idle,
    Alert,
    Trace,
    Revert,
    AttackWait,
    Attack,
    Death,
    End
}

public enum PlayerStateType
{
    Idle,
    Move,
    Dash,
    Attack,
    AttackWait,
    Death,
    End
}

public enum MonsterStateType
{
    None,
    Idle,
    Hit,
    Stun,
    Death,
    End
}