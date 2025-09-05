using UnityEngine;

namespace Code.Logic.Grid
{
    public class Cell : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [Space(10)] [Header("Colors")]
        [SerializeField] private Color _emptyColor;
        [SerializeField] private Color _fulledColor;
        [SerializeField] private Color _selectedColor;
        
        private TypeStateCell _stateCell;

        public void Initialize()
        {
            SetEmptyState();
        }

        public TypeStateCell StateCell => _stateCell;
        
        public void SetEmptyState()
        {
            _spriteRenderer.color = _emptyColor;
            _stateCell = TypeStateCell.Empty;
        }
        
        public void SetFulledState()
        {
            _spriteRenderer.color = _fulledColor;
            _stateCell = TypeStateCell.Fulled;
        }
        
        public void SetSelectedState()
        {
            _spriteRenderer.color = _selectedColor;
            _stateCell = TypeStateCell.Selected;
        }
    }
}