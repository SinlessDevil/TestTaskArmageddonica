using Code.Infrastructure.StateMachine.Game.States;
using Code.Logic.Grid;
using Code.Services.Context;
using Code.Services.LocalProgress;

namespace Code.Infrastructure.StateMachine.Battle.States
{
    public class CleanupBattleState : IState, IBattleState, IUpdatable
    {
        private readonly ILevelLocalProgressService _levelLocalProgressService;
        private readonly IGameContext _gameContext;
        private readonly IStateMachine<IBattleState> _stateMachine;

        public CleanupBattleState(
            ILevelLocalProgressService levelLocalProgressService, 
            IGameContext gameContext,
            IStateMachine<IBattleState> stateMachine)
        {
            _levelLocalProgressService = levelLocalProgressService;
            _gameContext = gameContext;
            _stateMachine = stateMachine;
        }

        void IState.Enter()
        {
            _levelLocalProgressService.ClearEnemyInvocationsDTO();
            CleanupEnemiesInvocations();
            
            _stateMachine.Enter<CardSelectionBattleState>();
        }

        void IExitable.Exit()
        {
            
        }
        
        public void Update()
        {
            
        }

        private void CleanupEnemiesInvocations()
        {
            Cell[,] cells = _gameContext.EnemyGird.Cells;
            
            int rows = cells.GetLength(0);
            int columns = cells.GetLength(1);
    
            for (int x = 0; x < rows; x++)
            for (int y = 0; y < columns; y++) 
                cells[x, y].InvocationController.ClearInvocations();
        }
    }
}