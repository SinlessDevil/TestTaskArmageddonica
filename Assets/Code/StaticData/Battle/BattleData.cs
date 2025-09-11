using System;
using UnityEngine;

namespace Code.StaticData.Battle
{
    [Serializable]
    public class BattleData
    {
        [Header("Battle Info")]
        [SerializeField] public string _battleName = "New Battle";
        [SerializeField] private int _battleId;
        [Header("Battle Matrix")]
        [SerializeField] private int _matrixWidth = 5;
        [SerializeField] private int _matrixHeight = 5;
        [SerializeField] private BattleMatrixCell[] _battleMatrix;
        
        public string BattleName => _battleName;
        public int BattleId => _battleId;
        public int MatrixWidth => _matrixWidth;
        public int MatrixHeight => _matrixHeight;
        public BattleMatrixCell[] BattleMatrix => _battleMatrix;
        
        public BattleData()
        {
            _battleId = 0;
        }
        
        public BattleData(string battleName, int width, int height)
        {
            _battleName = battleName;
            _battleId = 0;
            _matrixWidth = width;
            _matrixHeight = height;
        }
        
        public void GenerateBattleId()
        {
            if (_battleId == 0)
            {
                _battleId = UnityEngine.Random.Range(1000, 9999);
            }
        }
        
        public void InitializeMatrix()
        {
            BattleMatrixCell[] oldMatrix = _battleMatrix;
            int newSize = _matrixWidth * _matrixHeight;
            
            _battleMatrix = new BattleMatrixCell[newSize];
            
            if (oldMatrix != null)
            {
                int copySize = Mathf.Min(newSize, oldMatrix.Length);
                for (int i = 0; i < copySize; i++)
                {
                    _battleMatrix[i] = oldMatrix[i] ?? new BattleMatrixCell();
                }
            }
            
            for (int i = 0; i < newSize; i++)
            {
                _battleMatrix[i] ??= new BattleMatrixCell();
            }
        }
        
        public void ResizeMatrix(int newWidth, int newHeight)
        {
            if (newWidth <= 0 || newHeight <= 0) 
                return;
            
            BattleMatrixCell[] oldMatrix = _battleMatrix;
            int newSize = newWidth * newHeight;
            _battleMatrix = new BattleMatrixCell[newSize];
            
            if (oldMatrix != null)
            {
                int copyWidth = Mathf.Min(_matrixWidth, newWidth);
                int copyHeight = Mathf.Min(_matrixHeight, newHeight);
                
                for (int x = 0; x < copyWidth; x++)
                for (int y = 0; y < copyHeight; y++)
                {
                    int oldIndex = y * _matrixWidth + x;
                    int newIndex = y * newWidth + x;
                    if (oldIndex < oldMatrix.Length && newIndex < newSize)
                    {
                        _battleMatrix[newIndex] = oldMatrix[oldIndex];
                    }
                }
            }
            
            for (int i = 0; i < newSize; i++)
            {
                _battleMatrix[i] ??= new BattleMatrixCell();
            }
            
            _matrixWidth = newWidth;
            _matrixHeight = newHeight;
        }
        
        public BattleMatrixCell GetCell(int x, int y)
        {
            if (x >= 0 && x < _matrixWidth && y >= 0 && y < _matrixHeight)
            {
                int index = y * _matrixWidth + x;
                if (index < _battleMatrix.Length)
                    return _battleMatrix[index];
            }
            
            return null;
        }
        
        public void SetCell(int x, int y, BattleMatrixCell cell)
        {
            if (x >= 0 && x < _matrixWidth && y >= 0 && y < _matrixHeight) 
            {
                int index = y * _matrixWidth + x;
                if (index < _battleMatrix.Length)
                    _battleMatrix[index] = cell;
            }
        }
    }
}
