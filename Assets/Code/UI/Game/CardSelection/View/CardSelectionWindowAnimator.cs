using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;

namespace Code.UI.Game.CardSelection.View
{
    public class CardSelectionWindowAnimator : MonoBehaviour
    {
        [SerializeField] private List<CanvasGroup> _canvasGroups;
        [SerializeField] private RectTransform _rollCardsButton;
        [SerializeField] private RectTransform _selectButton;
        [SerializeField] private float _duration = 0.2f;
        [SerializeField] private Ease _ease = Ease.OutQuad;
        [SerializeField] private float _buttonMoveDistance = 100f;

        private Sequence _sequence;
        private Vector2 _rollCardsButtonStartPos;
        private Vector2 _selectButtonStartPos;

        public void Initialize()
        {
            _rollCardsButtonStartPos = _rollCardsButton.anchoredPosition;
            
            _selectButtonStartPos = _selectButton.anchoredPosition;
            
            foreach (CanvasGroup canvasGroup in _canvasGroups)
            {
                canvasGroup.alpha = 0f;
                canvasGroup.blocksRaycasts = false;
                canvasGroup.interactable = false;
            }
        }
        
        public void ShowCanvas() => CanvasToggleVisible(true, null);

        public void HideCanvas() => CanvasToggleVisible(false, null);

        public void HideCanvas(Action onCompleted) => CanvasToggleVisible(false, onCompleted);

        public void ShowButton() => ButtonsToggleVisible(true, null);
        
        public void HideButton() => ButtonsToggleVisible(false, null);
        
        private void CanvasToggleVisible(bool visible, Action onCompleted)
        {
            if (_canvasGroups == null || _canvasGroups.Count == 0)
            {
                onCompleted?.Invoke();
                return;
            }

            if (_sequence != null && _sequence.IsActive())
            {
                _sequence.Kill();
                _sequence = null;
            }

            foreach (var canvasGroup in _canvasGroups.Where(canvasGroup => canvasGroup))
            {
                canvasGroup.blocksRaycasts = false;
                canvasGroup.interactable = false;
            }

            Sequence sequence = DOTween.Sequence().SetUpdate(true);
            _sequence = sequence;
            
            foreach (var canvasGroup in _canvasGroups.Where(canvasGroup => canvasGroup))
            {
                sequence.Join(canvasGroup
                    .DOFade(visible ? 1f : 0f, _duration)
                    .SetEase(_ease));
            }

            sequence.OnComplete(() =>
            {
                foreach (var canvasGroup in _canvasGroups.Where(canvasGroup => canvasGroup))
                {
                    canvasGroup.blocksRaycasts = visible;
                    canvasGroup.interactable = visible;
                }
                onCompleted?.Invoke();
                _sequence = null;
            });
        }

        private void ButtonsToggleVisible(bool visible, Sequence sequence)
        {
            float buttonAnimationDuration = _duration * 0.5f;
            
            if (_rollCardsButton != null)
            {
                Vector2 targetPos = visible ? _rollCardsButtonStartPos : 
                    new Vector2(_rollCardsButtonStartPos.x - _buttonMoveDistance, _rollCardsButtonStartPos.y);
                
                sequence.Join(_rollCardsButton
                    .DOAnchorPos(targetPos, buttonAnimationDuration)
                    .SetEase(_ease));
            }

            if (_selectButton != null)
            {
                Vector2 targetPos = visible ? _selectButtonStartPos : 
                    new Vector2(_selectButtonStartPos.x + _buttonMoveDistance, _selectButtonStartPos.y);
                
                sequence.Join(_selectButton
                    .DOAnchorPos(targetPos, buttonAnimationDuration)
                    .SetEase(_ease));
            }
        }
    }
}

