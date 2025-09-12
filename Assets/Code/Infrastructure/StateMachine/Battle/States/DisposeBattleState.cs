using Code.Infrastructure.StateMachine.Game.States;
using Code.Logic.Grid;
using Code.Services.Context;
using Code.Services.LocalProgress;

namespace Code.Infrastructure.StateMachine.Battle.States
{
    public class DisposeBattleState : IState, IBattleState, IUpdatable
    {
        private readonly ILevelLocalProgressService _levelLocalProgressService;
        private readonly IGameContext _gameContext;
        private readonly IStateMachine<IBattleState> _stateMachine;

        public DisposeBattleState(
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
            _levelLocalProgressService.Cleanup();
            CleanupInvocations();
        }

        void IExitable.Exit()
        {
            
        }
        
        public void Update()
        {
            
        }

        private void CleanupInvocations()
        {
            Cell[,] enemyGirdCells = _gameContext.EnemyGird.Cells;
            
            int rows = enemyGirdCells.GetLength(0);
            int columns = enemyGirdCells.GetLength(1);
    
            for (int x = 0; x < rows; x++)
            for (int y = 0; y < columns; y++) 
                enemyGirdCells[x, y].InvocationController.ClearInvocations();
            
            Cell[,] playerGridCells = _gameContext.PlayerGrid.Cells;

            rows = playerGridCells.GetLength(0);
            columns = playerGridCells.GetLength(1);
            
            for (int x = 0; x < rows; x++)
            for (int y = 0; y < columns; y++) 
                playerGridCells[x, y].InvocationController.ClearInvocations();
        }
    }
}