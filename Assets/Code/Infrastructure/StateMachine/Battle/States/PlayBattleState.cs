using Code.Infrastructure.StateMachine.Game.States;
using Code.Logic.Grid;
using Code.Logic.Invocation;
using Code.Services.CameraController;
using Code.Services.Context;
using Code.Services.IInvocation.Factories;
using Code.Services.IInvocation.Randomizer;
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
        private readonly ICardInputService _cardInputService;
        private readonly ICameraDirector _cameraDirector;
        private readonly IGameContext _gameContext;
        private readonly IInvocationFactory _invocationFactory;
        private readonly IRandomizerService _randomizerService;
        private readonly ILevelConductor _levelConductor;
        private readonly ILevelService _levelService;
        private readonly IStaticDataService _staticDataService;

        public PlayBattleState(
            IGridInputService gridInputService,
            ICardInputService cardInputService,
            ICameraDirector cameraDirector,
            IGameContext gameContext,
            IInvocationFactory invocationFactory,
            IRandomizerService randomizerService,
            ILevelConductor levelConductor,
            ILevelService levelService,
            IStaticDataService staticDataService)
        {
            _gridInputService = gridInputService;
            _cardInputService = cardInputService;
            _cameraDirector = cameraDirector;
            _gameContext = gameContext;
            _invocationFactory = invocationFactory;
            _randomizerService = randomizerService;
            _levelConductor = levelConductor;
            _levelService = levelService;
            _staticDataService = staticDataService;
        }

        void IState.Enter()
        {
            _gridInputService.Disable();
            _cardInputService.Disable();
            
            _cameraDirector.FocusBattleShotAsync();
            
            SpawnEnemies();
        }

        void IExitable.Exit()
        {
            
        }
        
        public void Update()
        {
            
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
                    
                    InvocationDTO enemyDTO = CreateEnemyDTO(cell.InvocationId);
                    if (enemyDTO == null) 
                        continue;
                    
                    Invocation enemyInvocation = _invocationFactory.CreateInvocationByType(enemyDTO, enemyCell, HeadRotation.EnemyRotation);
                    if (enemyInvocation == null) 
                        continue;
                        
                    enemyCell.SetInvocation(enemyInvocation);
                    _levelConductor.AddInvocationForEnemy(enemyDTO);
                }
            }
        }
        
        private InvocationDTO CreateEnemyDTO(string invocationId) => _randomizerService.GenerateRandomInvocationDTO();

        private HeadRotation HeadRotation => _staticDataService.Balance.HeadRotation;
        private EnemyGird EnemyGrid => _gameContext.EnemyGird;
        private LevelStaticData LevelStaticData => _levelService.GetCurrentLevelStaticData();
        private int CurrentWave => _levelConductor.GetCurrentWave;
        private BattleData BattleData => LevelStaticData.BattleStaticData.BattleDataList[CurrentWave - 1];
    }
}