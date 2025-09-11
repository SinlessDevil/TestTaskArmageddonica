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
        [SerializeField] private BattleMatrixCell[,] _battleMatrix;
        
        public string BattleName => _battleName;
        public int BattleId => _battleId;
        public int MatrixWidth => _matrixWidth;
        public int MatrixHeight => _matrixHeight;
        public BattleMatrixCell[,] BattleMatrix => _battleMatrix;
        
        public BattleData()
        {
            _battleId = 0; // Будет сгенерирован позже
            InitializeMatrix();
        }
        
        public BattleData(string battleName, int width, int height)
        {
            _battleName = battleName;
            _battleId = 0; // Будет сгенерирован позже
            _matrixWidth = width;
            _matrixHeight = height;
            
            InitializeMatrix();
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
            // Сохраняем существующие данные если матрица уже существует
            BattleMatrixCell[,] oldMatrix = _battleMatrix;
            
            _battleMatrix = new BattleMatrixCell[_matrixWidth, _matrixHeight];
            
            // Копируем существующие данные если они есть
            if (oldMatrix != null)
            {
                int copyWidth = Mathf.Min(_matrixWidth, oldMatrix.GetLength(0));
                int copyHeight = Mathf.Min(_matrixHeight, oldMatrix.GetLength(1));
                
                for (int x = 0; x < copyWidth; x++)
                for (int y = 0; y < copyHeight; y++)
                {
                    _battleMatrix[x, y] = oldMatrix[x, y] ?? new BattleMatrixCell();
                }
            }
            
            // Заполняем новые ячейки
            for (int x = 0; x < _matrixWidth; x++)
            for (int y = 0; y < _matrixHeight; y++)
            {
                if (_battleMatrix[x, y] == null)
                {
                    _battleMatrix[x, y] = new BattleMatrixCell();
                }
            }
        }
        
        public void ResizeMatrix(int newWidth, int newHeight)
        {
            if (newWidth <= 0 || newHeight <= 0) 
                return;
            
            BattleMatrixCell[,] newMatrix = new BattleMatrixCell[newWidth, newHeight];
            
            for (int x = 0; x < Mathf.Min(_matrixWidth, newWidth); x++)
            for (int y = 0; y < Mathf.Min(_matrixHeight, newHeight); y++) 
                newMatrix[x, y] = _battleMatrix[x, y];
            
            for (int x = 0; x < newWidth; x++) 
            for (int y = 0; y < newHeight; y++) 
                newMatrix[x, y] ??= new BattleMatrixCell();
            
            _battleMatrix = newMatrix;
            _matrixWidth = newWidth;
            _matrixHeight = newHeight;
        }
        
        public BattleMatrixCell GetCell(int x, int y)
        {
            if (x >= 0 && x < _matrixWidth && y >= 0 && y < _matrixHeight)
                return _battleMatrix[x, y];
            
            return null;
        }
        
        public void SetCell(int x, int y, BattleMatrixCell cell)
        {
            if (x >= 0 && x < _matrixWidth && y >= 0 && y < _matrixHeight) 
                _battleMatrix[x, y] = cell;
        }
    }
}
