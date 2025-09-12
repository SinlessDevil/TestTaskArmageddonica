using Code.Extensions;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Code.UI.Game.Cards.View
{
    public sealed class CardViewHover : MonoBehaviour
    {
        [Header("Tween")]
        [SerializeField] private float _tweenDuration = 0.12f;
        [SerializeField] private Ease _tweenEase = Ease.OutQuad;
        [SerializeField] private bool _useUnscaledTime = true;
        [Header("Lift")]
        [SerializeField] private float _startYOffset = -75f;
        [SerializeField] private float _endYOffset = 0f;
        [Header("Scale")]
        [SerializeField] private float _hoverScale = 1.07f;
        [SerializeField] private float _shrinkScale = 0.55f;
        [Header("Refs")]
        [SerializeField] private RectTransform _root;
        [SerializeField] private Image _bgImage;

        private Material _bgMaterial;
        private float _baseScale = 1f;

        private void Awake()
        {
            if (_root) 
                _baseScale = _root.localScale.x;

            if (!_bgImage || !_bgImage.material) 
                return;
            
            _bgMaterial = new Material(_bgImage.material);
            _bgImage.material = _bgMaterial;
        }

        private void OnDisable()
        {
            KillTweens();
        }
        
        public void HighlightOn()
        {
            SetGlow(true);
            AnimateScale(_hoverScale);
        }

        public void HighlightOff()
        {
            SetGlow(false);
            AnimateScale(_baseScale);
        }

        public void HoverEnter()
        {
            AnimateLift(_endYOffset);
            transform.SetAsLastSibling();
            SetGlow(true);
            AnimateScale(_hoverScale);
        }

        public void HoverExit()
        {
            AnimateLift(_startYOffset);
            SetGlow(false);
            AnimateScale(_baseScale);
        }

        public void ResetState()
        {
            AnimateLift(0);
            SetGlow(false);
            AnimateScale(_baseScale);
        }

        public void HighlightShrink()
        {
            SetGlow(true);
            AnimateScale(_shrinkScale);
        }

        private void AnimateLift(float targetY)
        {
            _root.DOKill(true);
            
            Tweener tweener = _root
                .DOAnchorPos(new Vector2(_root.anchoredPosition.x, targetY), _tweenDuration)
                .SetEase(_tweenEase);
            
            if (_useUnscaledTime) 
                tweener.SetUpdate(true);
        }

        private void AnimateScale(float targetScale)
        {
            if (!_root) 
                return;

            _root.DOKill(true);
            Tweener tweener = _root
                .DOScale(targetScale, _tweenDuration)
                .SetEase(_tweenEase);
            
            if (_useUnscaledTime) 
                tweener.SetUpdate(true);
        }

        private void SetGlow(bool on)
        {
            if (_bgMaterial == null) 
                return;

            _bgMaterial.SetGlowEnabled(on, resetPhase: true);
        }

        private void KillTweens()
        {
            if (!_root) 
                return;
            
            _root.DOKill(true);
            transform.DOKill(true);
        }
    }
}
