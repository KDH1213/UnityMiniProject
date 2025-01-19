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

public enum EnemyStateType
{
    None,
    Idle,
    Alert,
    Trace,
    Revert,
    Attack, 
    AttackWait,
    Death,
    End
}