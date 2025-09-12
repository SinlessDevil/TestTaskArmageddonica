using System.Collections.Generic;
using System.Linq;
using Code.UI.Game.Finish.InvocationIcon.Extensions;
using UnityEngine;

namespace Code.UI.Game.Finish.InvocationIcon
{
    public class InvocationLayoutEngine : MonoBehaviour
    {
        [Header("Icon Positioning")]
        [SerializeField] private float _iconSpacing = 120f;
        [SerializeField] private float _verticalOffset = 0f;  
        [Header("Icon Settings")]
        [SerializeField] private float _iconWidth = 200f;
        [Header("Components")]
        [SerializeField] private RectTransform _root;
        
        private List<InvocationIconView> _iconViews = new();

        public void Initialize()
        {
            UpdateIconPositions();
        }
        
        public void AddIcon(InvocationIconView iconView)
        {
            if (iconView == null) 
                return;
            
            iconView.transform.SetParent(_root);
            iconView.transform.localScale = Vector3.one;

            _iconViews.Add(iconView);
            UpdateIconPositions();
        }
        
        public void RemoveIcon(InvocationIconView iconView)
        {
            if (!iconView) 
                return;

            _iconViews.Remove(iconView);
            iconView.transform.SetParent(null);
            UpdateIconPositions();
        }
        
        public void ClearAllIcons()
        {
            foreach (var iconView in _iconViews.Where(iconView => iconView))
            {
                iconView.transform.SetParent(null);
            }

            _iconViews.Clear();
        }
        
        private void UpdateIconPositions()
        {
            int iconCount = _iconViews.Count;
            if (iconCount == 0) 
                return;

            Vector2[] positions = InvocationLayoutExtensions.CalculateCenteredRowPositions(_root, iconCount, _iconWidth,
                _iconSpacing, _verticalOffset);
            
            for (int i = 0; i < iconCount; i++)
            {
                if (_iconViews[i] == null) 
                    continue;
                
                RectTransform iconRect = _iconViews[i].GetComponent<RectTransform>();
                iconRect.anchoredPosition = positions[i];
            }
        }
    }
}
