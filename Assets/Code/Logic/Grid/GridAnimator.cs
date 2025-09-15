using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;

namespace Code.Logic.Grid
{
    public class GridAnimator : MonoBehaviour
    {
        [Header("Animation Settings")]
        [SerializeField] private ParticleSystem _battleParticleSystem;
        [SerializeField] private float _animationDuration = 0.5f;
        
        private Cell.Cell[,] _cells;
        private Vector3 _gridCenter;
        
        public void Initialize(Cell.Cell[,] cells)
        {
            _cells = cells;
            CalculateGridCenter();
        }
        
        public async UniTask PlayBattleAnimation()
        {
            _battleParticleSystem.Play();
            await UniTask.Delay(1000);
            await AnimateObjectsInCircle();
        }
        
        private void CalculateGridCenter()
        {
            if (_cells == null || _cells.Length == 0)
            {
                _gridCenter = transform.position;
                return;
            }
            
            Vector3 totalPosition = Vector3.zero;
            int cellCount = 0;
            
            for (int x = 0; x < _cells.GetLength(0); x++)
            for (int y = 0; y < _cells.GetLength(1); y++)
            {
                if (_cells[x, y] != null)
                {
                    totalPosition += _cells[x, y].transform.position;
                    cellCount++;
                }
            }
            
            _gridCenter = cellCount > 0 ? totalPosition / cellCount : transform.position;
        }
        
        private async UniTask AnimateObjectsInCircle()
        {
            List<Cell.Cell> cellsToAnimate = GetCellsToAnimate();
            
            if (cellsToAnimate.Count == 0)
                return;
            
            Dictionary<int, List<Cell.Cell>> rings = GroupCellsByRings(cellsToAnimate);
            for (int ringIndex = 0; ringIndex < rings.Count; ringIndex++)
            {
                List<Cell.Cell> ringCells = rings[ringIndex];
                List<UniTask> ringAnimations = new List<UniTask>();
                foreach (Cell.Cell cell in ringCells) 
                    ringAnimations.Add(AnimateCellLiftAndDrop(cell));
                
                await UniTask.WhenAll(ringAnimations);
                if (ringIndex < rings.Count - 1) 
                    await UniTask.Delay(100);
            }
        }
        
        private List<Cell.Cell> GetCellsToAnimate()
        {
            List<Cell.Cell> cells = new List<Cell.Cell>();
            
            for (int x = 0; x < _cells.GetLength(0); x++)
            for (int y = 0; y < _cells.GetLength(1); y++)
            {
                Cell.Cell cell = _cells[x, y];
                if (cell != null)
                    cells.Add(cell);
            }
            
            return cells;
        }
        
        private Dictionary<int, List<Cell.Cell>> GroupCellsByRings(List<Cell.Cell> cells)
        {
            Dictionary<int, List<Cell.Cell>> rings = new Dictionary<int, List<Cell.Cell>>();
            
            foreach (Cell.Cell cell in cells)
            {
                float distance = Vector3.Distance(_gridCenter, cell.transform.position);
                int ringIndex = Mathf.RoundToInt(distance / 2f);
                if (!rings.ContainsKey(ringIndex))
                    rings[ringIndex] = new List<Cell.Cell>();
                rings[ringIndex].Add(cell);
            }
            
            return rings;
        }
        
        private async UniTask AnimateCellLiftAndDrop(Cell.Cell cell)
        {
            Vector3 originalPosition = cell.transform.position;
            Vector3 liftedPosition = originalPosition + Vector3.up * 1.5f;
            Vector3 droppedPosition = originalPosition + Vector3.down * 1.5f;
            
            await cell.transform.DOMove(liftedPosition, _animationDuration * 0.5f).ToUniTask();
            await cell.transform.DOMove(droppedPosition, _animationDuration * 0.5f).ToUniTask();
        }
    }
}
