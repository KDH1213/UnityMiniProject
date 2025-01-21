public class MonsterBaseState : BaseState<MonsterStateType>
{
    // protected EnemyFSM enemyFSM;
    // public EnemyFSM EnemyFSM { get { return enemyFSM; } }

    public override void Enter() 
    {
        enterStateEvent?.Invoke();
        this.enabled = true;
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
        this.enabled = false;
    }
}
