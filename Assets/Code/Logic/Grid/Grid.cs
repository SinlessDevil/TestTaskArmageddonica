using UnityEngine;

namespace Code.Logic.Grid
{
    public abstract class Grid : MonoBehaviour
    {
        private Cell[,] _cells;
        
        public void Initialize(Cell[,] cells)
        {
            _cells = cells;
        }
        
        public Cell GetCell(int x, int y)
        {
            if (x < 0 || y < 0 || x >= _cells.GetLength(0) || y >= _cells.GetLength(1))
                return null;
            
            return _cells[x, y];
        }
    }   
}