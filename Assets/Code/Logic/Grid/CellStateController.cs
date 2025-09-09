using UnityEngine;

namespace Code.Logic.Grid
{
    public class CellStateController : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [Space(10)] [Header("Colors")]
        [SerializeField] private Color _emptyColor;
        [SerializeField] private Color _fulledColor;
        [SerializeField] private Color _selectedColor;
        
        private TypeStateCell _currentState;

        public TypeStateCell CurrentState => _currentState;
        
        public void Initialize()
        {
            SetEmptyState();
        }
        
        public void SetEmptyState()
        {
            if (_spriteRenderer != null)
                _spriteRenderer.color = _emptyColor;
            _currentState = TypeStateCell.Empty;
        }
        
        public void SetFulledState()
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
        
        public void SetState(TypeStateCell state)
        {
            switch (state)
            {
                case TypeStateCell.Empty:
                    SetEmptyState();
                    break;
                case TypeStateCell.Fulled:
                    SetFulledState();
                    break;
                case TypeStateCell.Selected:
                    SetSelectedState();
                    break;
            }
        }
    }
}
