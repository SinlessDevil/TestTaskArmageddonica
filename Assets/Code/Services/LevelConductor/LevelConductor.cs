using Code.Infrastructure.StateMachine;
using Code.Infrastructure.StateMachine.Battle;
using Code.Infrastructure.StateMachine.Battle.States;

namespace Code.Services.LevelConductor
{
    public class LevelConductor : ILevelConductor
    {
        private readonly IStateMachine<IBattleState> _battleStateMachine;
        
        public LevelConductor(IStateMachine<IBattleState> battleStateMachine)
        {
            _battleStateMachine = battleStateMachine;
        }

        public void Run()
        {
            _battleStateMachine.Enter<InitializeBattleState>();
        }

        public void Cleanup()
        {
            _battleStateMachine.Enter<CleanupBattleState>();
        }
    }   
}