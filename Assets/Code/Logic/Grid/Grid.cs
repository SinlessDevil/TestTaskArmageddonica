using UnityEngine;

namespace Code.Logic.Grid
{
    public abstract class Grid : MonoBehaviour
    {
        [SerializeField] private GridAnimator _gridAnimator;
        
        private Cell.Cell[,] _cells;
        
        public void Initialize(Cell.Cell[,] cells)
        {
            _cells = cells;
            _gridAnimator.Initialize(_cells);
        }
        
        public GridAnimator GridAnimator => _gridAnimator;
        
        public Cell.Cell[,] Cells => _cells;
        
        public Cell.Cell GetCell(int x, int y)
        {
            if (x < 0 || y < 0 || x >= _cells.GetLength(0) || y >= _cells.GetLength(1))
                return null;
            
            return _cells[x, y];
        }
    }   
}