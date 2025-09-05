using UnityEngine;

namespace Code.Logic.Grid
{
    public class Grid : MonoBehaviour
    {
        private Cell[,] _cells;
        
        public void Initialize(Cell[,] cells)
        {
            _cells = cells;
        }
    }   
}