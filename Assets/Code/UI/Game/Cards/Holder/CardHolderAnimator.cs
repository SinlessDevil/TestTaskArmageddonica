using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

namespace Code.UI.Game.Cards.Holder
{
    public class CardHolderAnimator : MonoBehaviour
    {
        [Header("Settings")] 
        [SerializeField] private float _shownY = 0f;
        [SerializeField] private float _hiddenY = -200f;
        [SerializeField] private float _duration = 0.25f;
        [SerializeField] private Ease _ease = Ease.OutCubic;
        [Header("Components")] 
        [SerializeField] private RectTransform _root;
        [SerializeField] private CanvasGroup _canvasGroup;

        private Tween _moveTween;
        private Tween _fadeTween;
        private bool _isVisible;

        public void Initialize()
        {
            _isVisible = false;
            _root.anchoredPosition = new Vector2(_root.anchoredPosition.x, _hiddenY);
            _canvasGroup.alpha = 0f;
        }
        
        [Button]
        public void Show()
        {
            if (_isVisible) 
                return;
            _isVisible = true;
            
            KillTweens();

            _root.gameObject.SetActive(true);

            _canvasGroup.alpha = Mathf.Clamp01(_canvasGroup.alpha);
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;

            _moveTween = _root.DOAnchorPosY(_shownY, _duration)
                .SetEase(_ease)
                .SetUpdate(true);

            _fadeTween = _canvasGroup.DOFade(1f, _duration)
                .SetEase(_ease)
                .SetUpdate(true);
        }

        [Button]
        public void Hide()
        {
            if (!_isVisible) 
                return;
            
            _isVisible = false;
            
            KillTweens();

            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;

            _moveTween = _root.DOAnchorPosY(_hiddenY, _duration)
                .SetEase(_ease)
                .SetUpdate(true);

            _fadeTween = _canvasGroup.DOFade(0f, _duration)
                .SetEase(_ease)
                .SetUpdate(true);
            
        }

        private void KillTweens()
        {
            _moveTween?.Kill();
            _fadeTween?.Kill();
        }
    }
}