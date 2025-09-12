using Code.Infrastructure.StateMachine.Game.States;
using Code.Logic.Grid;
using Code.Logic.Invocation;
using Code.Services.CameraController;
using Code.Services.Context;
using Code.Services.IInvocation.Creator;
using Code.Services.IInvocation.Factories;
using Code.Services.Input.Card;
using Code.Services.Input.Grid;
using Code.Services.LevelConductor;
using Code.Services.Levels;
using Code.Services.StaticData;
using Code.StaticData;
using Code.StaticData.Battle;
using Code.StaticData.Invocation.DTO;
using Code.StaticData.Levels;

namespace Code.Infrastructure.StateMachine.Battle.States
{
    public class PlayBattleState : IState, IBattleState, IUpdatable
    {
        private readonly IGridInputService _gridInputService;
        private readonly IDragCardInputService _dragCardInputService;
        private readonly ICameraDirector _cameraDirector;
        private readonly IGameContext _gameContext;
        private readonly IInvocationFactory _invocationFactory;
        private readonly IInvocationCreatorDTOService _invocationCreatorDtoService;
        private readonly ILevelConductor _levelConductor;
        private readonly ILevelService _levelService;
        private readonly IStaticDataService _staticDataService;
        private readonly IStateMachine<IBattleState> _stateMachine;
        
        public PlayBattleState(
            IGridInputService gridInputService,
            IDragCardInputService dragCardInputService,
            ICameraDirector cameraDirector,
            IGameContext gameContext,
            IInvocationFactory invocationFactory,
            IInvocationCreatorDTOService invocationCreatorDtoService,
            ILevelConductor levelConductor,
            ILevelService levelService,
            IStaticDataService staticDataService,
            IStateMachine<IBattleState> stateMachine)
        {
            _gridInputService = gridInputService;
            _dragCardInputService = dragCardInputService;
            _cameraDirector = cameraDirector;
            _gameContext = gameContext;
            _invocationFactory = invocationFactory;
            _invocationCreatorDtoService = invocationCreatorDtoService;
            _levelConductor = levelConductor;
            _levelService = levelService;
            _staticDataService = staticDataService;
            _stateMachine = stateMachine;
        }

        void IState.Enter()
        {
            _gridInputService.Disable();
            _dragCardInputService.Disable();
            
            _cameraDirector.FocusBattleShotAsync();
            
            SpawnEnemies();
            
            _levelConductor.RunBattle();
            _levelConductor.EndedBattleEvent += OnNextWave;
        }

        void IExitable.Exit()
        {
            _levelConductor.EndedBattleEvent -= OnNextWave;
        }
        
        public void Update()
        {
            
        }

        private void OnNextWave()
        {
            _stateMachine.Enter<CleanupBattleState>();
        }
        
        private void SpawnEnemies()
        {
            for (int x = 0; x < BattleData.MatrixWidth; x++)
            {
                for (int y = 0; y < BattleData.MatrixHeight; y++)
                {
                    BattleMatrixCell cell = BattleData.GetCell(x, y);
                    Cell enemyCell = EnemyGrid.GetCell(x, y);
                    if (cell == null || !cell.IsOccupied || string.IsNullOrEmpty(cell.InvocationId)) 
                        continue;
                    
                    if (enemyCell == null) 
                        continue;
                    
                    if (cell.InvocationId == "")
                        continue;
                    
                    InvocationDTO enemyDTO = CreateEnemyDTO(cell.InvocationId);
                    if (enemyDTO == null) 
                        continue;
                    
                    Invocation enemyInvocation = _invocationFactory.CreateInvocationByType(enemyDTO, enemyCell, HeadRotation.EnemyRotation);
                    if (enemyInvocation == null) 
                        continue;
                        
                    enemyCell.InvocationController.AddInvocation(enemyInvocation, enemyDTO.InvocationType, enemyDTO.UniqueId);
                    _levelConductor.AddInvocationForEnemy(enemyDTO);
                }
            }
        }
        
        private InvocationDTO CreateEnemyDTO(string invocationId) => 
            _invocationCreatorDtoService.GetInvocationDTO(invocationId);

        private HeadRotation HeadRotation => _staticDataService.Balance.HeadRotation;
        private EnemyGird EnemyGrid => _gameContext.EnemyGird;
        private LevelStaticData LevelStaticData => _levelService.GetCurrentLevelStaticData();
        private int CurrentWave => _levelConductor.GetCurrentWave;
        private BattleData BattleData => LevelStaticData.BattleStaticData.BattleDataList[CurrentWave - 1];
    }
}