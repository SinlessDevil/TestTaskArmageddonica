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
        
        private Cell[,] _cells;
        private Vector3 _gridCenter;
        
        public void Initialize(Cell[,] cells)
        {
            _cells = cells;
            CalculateGridCenter();
        }
        
        public async UniTask PlayBattleAnimation()
        {
            _battleParticleSystem.Play();
            await UniTask.Delay(1500);
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
            List<Cell> cellsToAnimate = GetCellsToAnimate();
            
            if (cellsToAnimate.Count == 0)
                return;
            
            List<UniTask> animationTasks = new List<UniTask>();
            
            for (int i = 0; i < cellsToAnimate.Count; i++)
            {
                Cell cell = cellsToAnimate[i];
                animationTasks.Add(AnimateCellLiftAndDrop(cell));
            }
            
            await UniTask.WhenAll(animationTasks);
        }
        
        private List<Cell> GetCellsToAnimate()
        {
            List<Cell> cells = new List<Cell>();
            
            for (int x = 0; x < _cells.GetLength(0); x++)
            for (int y = 0; y < _cells.GetLength(1); y++)
            {
                Cell cell = _cells[x, y];
                if (cell != null)
                    cells.Add(cell);
            }
            
            return cells;
        }
        
        private async UniTask AnimateCellLiftAndDrop(Cell cell)
        {
            Vector3 originalPosition = cell.transform.position;
            Vector3 liftedPosition = originalPosition + Vector3.up * 1.5f;
            Vector3 droppedPosition = originalPosition + Vector3.down * 1.5f;
            
            await cell.transform.DOMove(liftedPosition, _animationDuration * 0.5f).ToUniTask();
            await cell.transform.DOMove(droppedPosition, _animationDuration * 0.5f).ToUniTask();
        }
    }
}
