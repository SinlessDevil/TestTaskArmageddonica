namespace Code.Infrastructure.StateMachine.Battle
{
    public class BattleStateMachine : StateMachine<IBattleState>
    {
        public BattleStateMachine(BattleStateFactory stateFactory) : base(stateFactory)
        {
        }
    }
}