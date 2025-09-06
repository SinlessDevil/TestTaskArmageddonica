using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

namespace Code.UI.Game.Cards
{
    public class CardViewHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
    {
        private string GlowEnabledProp = "_AlphaClip";
        
        [Header("Hover")]
        [SerializeField] private float _hoverScale = 1.07f;
        [SerializeField] private float _tweenDuration = 0.12f;
        [SerializeField] private float _endYOffset = 0f;
        [SerializeField] private float _startYOffset = -75f;  
        [SerializeField] private Ease _tweenEase = Ease.Linear;
        [Header("Refs (assign in Inspector)")]
        [SerializeField] private RectTransform _root;
        [SerializeField] private Image _bgImage;
        
        private bool _isHovered, _isPressed;

        public void OnPointerEnter(PointerEventData e)
        {
            _isHovered = true;
            ApplyStateTween();
            transform.SetAsLastSibling();

            SetGlowEnabled(true);
        }

        public void OnPointerExit(PointerEventData e)
        {
            _isHovered = false;
            _isPressed = false;
            ApplyStateTween();

            SetGlowEnabled(false);
        }

        public void OnPointerDown(PointerEventData e)
        {
            _isPressed = true;
            ApplyStateTween();
        }

        public void OnPointerUp(PointerEventData e)
        {
            _isPressed = false;
            ApplyStateTween();
        }

        private void ApplyStateTween()
        {
            _root.DOKill();
            
            _root.DOAnchorPos(GetAnimatedTarget(), _tweenDuration)
                .SetEase(_tweenEase);
                
            float s = _isHovered ? _hoverScale : 1f;
            if (_isPressed) 
                s *= 0.98f;
            
            _root.DOScale(s, _tweenDuration)
                     .SetEase(_tweenEase)
                     .SetUpdate(true);
        }

        private Vector2 GetAnimatedTarget()
        {
            float x = _root.anchoredPosition.x;
            float y = _isHovered ? _endYOffset : _startYOffset;
            return new Vector2(x, y);
        }
        
        private void SetGlowEnabled(bool on)
        {
            if (!_bgImage) 
                return;

            Material material = _bgImage.material;
            if (material && material.HasProperty(GlowEnabledProp))
                material.SetFloat(GlowEnabledProp, on ? 0f : 1f); 
        }
    }
}
