using System;
using Code.Logic.Grid;
using UnityEngine;

namespace Code.Logic.Cell
{
    public class CellVisualController : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [Space(10)] [Header("Colors")]
        [SerializeField] private Color _emptyColor;
        [SerializeField] private Color _fulledColor;
        [SerializeField] private Color _selectedColor;
        [SerializeField] private Color _notSelectedColor = Color.red;
        
        private TypeStateCell _currentState;

        public TypeStateCell CurrentState => _currentState;
        
        public void Initialize()
        {
            SetEmptyState();
        }
        
        public TypeStateCell StateCell => _currentState;
        
        public void SetEmptyState()
        {
            if (_spriteRenderer != null)
                _spriteRenderer.color = _emptyColor;
            _currentState = TypeStateCell.Empty;
        }
        
        public void SetFilledState()
        {
            if (_spriteRenderer != null)
                _spriteRenderer.color = _fulledColor;
            _currentState = TypeStateCell.Fulled;
        }
        
        public void SetSelectedState()
        {
            if (_spriteRenderer != null)
                _spriteRenderer.color = _selectedColor;
            _currentState = TypeStateCell.Selected;
        }
        
        public void SetNotSelectedState()
        {
            if (_spriteRenderer != null)
                _spriteRenderer.color = _notSelectedColor;
            _currentState = TypeStateCell.NotSelected;
        }
        
        public void SetState(TypeStateCell state)
        {
            switch (state)
            {
                case TypeStateCell.Empty:
                    SetEmptyState();
                    break;
                case TypeStateCell.Fulled:
                    SetFilledState();
                    break;
                case TypeStateCell.Selected:
                    SetSelectedState();
                    break;
                case TypeStateCell.NotSelected:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }
    }
}
