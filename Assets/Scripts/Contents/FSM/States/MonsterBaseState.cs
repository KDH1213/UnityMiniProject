public class MonsterBaseState : BaseState<MonsterStateType>
{
    protected MonsterFSMController monsterFSM;
    public MonsterFSMController MonsterFSM { get { return monsterFSM; } }

    protected virtual void Awake()
    {
        monsterFSM = GetComponent<MonsterFSMController>();
    }

    public override void Enter() 
    {
        enterStateEvent?.Invoke();
    }
    public override void ExcuteUpdate()
    {
        excuteUpdateStateEvent?.Invoke();
    }
    public override void ExcuteFixedUpdate()
    {
        excuteFixedUpdateStateEvent?.Invoke();
    }

    public override void Exit() 
    {
        exitStateEvent?.Invoke();
    }
}
