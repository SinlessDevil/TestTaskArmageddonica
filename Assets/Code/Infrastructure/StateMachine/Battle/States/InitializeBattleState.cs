using Code.Infrastructure.StateMachine.Game.States;
using Code.Logic.Grid;
using Code.Services.Factories.Grid;
using Code.Services.Factories.UIFactory;
using Code.Services.Input.Card;
using Code.Services.Input.Grid;
using Code.Services.Levels;
using Code.StaticData.Levels;
using Code.UI.Game.Cards.Holder;
using Services.Contex;
using UnityEngine;
using Grid = Code.Logic.Grid.Grid;

namespace Code.Infrastructure.StateMachine.Battle.States
{
    public class InitializeBattleState : IState, IBattleState, IUpdatable
    {
        private float _step = 0.35f;
        private float _height = 0.01f;
        private Quaternion _rotationLocal = Quaternion.Euler(90f, 0f, 0f);
        
        private readonly IGridFactory _gridFactory;
        private readonly ILevelService _levelService;
        private readonly IGridInputService _gridInputService;
        private readonly ICardInputService _cardInputService;
        private readonly IGameContext _gameContext;
        private readonly IUIFactory _uiFactory;

        public InitializeBattleState(
            IGridFactory gridFactory, 
            ILevelService levelService,
            IGridInputService gridInputService,
            ICardInputService cardInputService,
            IGameContext gameContext,
            IUIFactory uiFactory)
        {
            _gridFactory = gridFactory;
            _levelService = levelService;
            _gridInputService = gridInputService;
            _cardInputService = cardInputService;
            _gameContext = gameContext;
            _uiFactory = uiFactory;
        }
        
        public void Enter()
        {
            _gridInputService.Disable();
            _cardInputService.Disable();
            
            CardHolder.Hide();
            
            InitGrid();
            
            
        }

        public void Exit()
        {
            
        }

        public void Update()
        {
            
        }

        private void InitGrid()
        {
            LevelStaticData levelData = GetCurrentLevelStaticData();
            GridData gridData = levelData.GridData;

            Cell[,] cells = GetCells(Grid.transform, gridData.Rows, gridData.Columns);
            Grid.Initialize(cells);
        }
        
        private Cell[,] GetCells(Transform root, int rows, int columns)
        {
            Cell[,] cells = new Cell[rows, columns];

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < columns; col++)
                {
                    Vector3 localPosition = new Vector3(col * _step, _height, row * _step);
                    Cell cell = _gridFactory.CreateCell(root.position, root.rotation, root);
                    Transform transform = cell.transform;
                    transform.localPosition = localPosition;
                    transform.localRotation = _rotationLocal;

                    cell.Initialize();
                    
                    cells[row, col] = cell;
                }
            }

            return cells;
        }
        
        private LevelStaticData GetCurrentLevelStaticData() => _levelService.GetCurrentLevelStaticData();
        
        private Grid Grid => _gameContext.Grid;
        
        private CardHolder CardHolder => _uiFactory.GameHud.CardHolder;
    }
}