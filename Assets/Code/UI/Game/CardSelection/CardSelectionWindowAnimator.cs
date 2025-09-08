using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Code.UI.Game.CardSelection
{
    public class CardSelectionWindowAnimator : MonoBehaviour
    {
        [SerializeField] private List<CanvasGroup> _canvasGroups;
        [SerializeField] private float _duration = 0.2f;
        [SerializeField] private Ease _ease = Ease.OutQuad;

        private Sequence _sequence;

        public void Initialize()
        {
            foreach (CanvasGroup canvasGroup in _canvasGroups)
            {
                canvasGroup.alpha = 0f;
                canvasGroup.blocksRaycasts = false;
                canvasGroup.interactable = false;
            }
        }
        
        public void Show() => Play(true, null);

        public void Hide() => Play(false, null);

        public void Close(Action onCompleted) => Play(false, onCompleted);

        private void Play(bool visible, Action onCompleted)
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
    }
}

