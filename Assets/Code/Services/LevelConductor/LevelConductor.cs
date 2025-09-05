using Code.Logic.Grid;
using Code.Services.Factories.Grid;
using Code.Services.Levels;
using Code.StaticData.Levels;
using UnityEngine;
using Grid = Code.Logic.Grid.Grid;

namespace Code.Services.LevelConductor
{
    public class LevelConductor : ILevelConductor
    {
        private float _step = 0.35f;
        private float _height = 0.01f;
        private Quaternion _rotationLocal = Quaternion.Euler(90f, 0f, 0f);
        
        private Grid _grid;
        
        private readonly IGridFactory _gridFactory;
        private readonly ILevelService _levelService;

        public LevelConductor(IGridFactory gridFactory, ILevelService levelService)
        {
            _gridFactory = gridFactory;
            _levelService = levelService;
        }
        
        public void Run()
        {
            
        }

        public void Setup()
        {
            Grid grid = GetGrid();
            _grid = grid;

            LevelStaticData levelData = GetCurrentLevelStaticData();
            GridData gridData = levelData.GridData;

            Cell[,] cells = GetCells(grid.transform, gridData.Rows, gridData.Columns);
            grid.Initialize(cells);
        }
        
        public void Dispose()
        {
            _grid = null;
        }

        private Cell[,] GetCells(Transform root)
        {
            var levelData = GetCurrentLevelStaticData();
            return GetCells(root, levelData.GridData.Rows, levelData.GridData.Columns);
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

                    cells[row, col] = cell;
                }
            }

            return cells;
        }

        private Grid GetGrid()
        {
            Grid grid = Object.FindAnyObjectByType<Grid>();
            return grid;
        }
        
        private LevelStaticData GetCurrentLevelStaticData() => 
            _levelService.GetCurrentLevelStaticData();
    }   
}