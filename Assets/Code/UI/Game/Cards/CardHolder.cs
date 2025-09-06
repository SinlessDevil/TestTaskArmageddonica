using System.Threading;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;

namespace Code.UI.Game.Cards
{
    public class CardHolder : MonoBehaviour
    {
        [Header("Card Positioning")]
        [SerializeField] private List<CardView> _cardViews = new();
        [SerializeField] private RectTransform _root;
        [SerializeField] private float _cardSpacing = 200f;
        [SerializeField] private float _verticalOffset = 0f;  
        [SerializeField] private float _animationDuration = 0.3f;
        [SerializeField] private Ease _animationEase = Ease.Linear;
        [Header("Card Settings")]
        [SerializeField] private float _cardWidth = 100f;
        [SerializeField] private int _maxCardsInRow = 7;

        private readonly List<RectTransform> _cardTransforms = new();
        
        private CancellationTokenSource _layoutCts;
        private Sequence _activeSequence;

        public void Initialize()
        {
            InitializeCardTransforms();
            UpdateCardPositionsImmediate();
        }

        [Button]
        public void AddCard(CardView cardView)
        {
            if (!cardView) 
                return;

            cardView.transform.SetParent(_root, false);

            var rt = cardView.GetComponent<RectTransform>();
            rt.anchoredPosition = Vector2.zero;
            rt.localScale = Vector3.one;

            _cardViews.Add(cardView);
            _cardTransforms.Add(rt);

            RestartLayoutAnimation();
        }

        [Button]
        public void RemoveCard(CardView cardView)
        {
            if (!cardView) 
                return;

            int index = _cardViews.IndexOf(cardView);
            if (index >= 0)
            {
                var rt = _cardTransforms[index];
                rt.DOKill();

                _cardViews.RemoveAt(index);
                _cardTransforms.RemoveAt(index);

                cardView.transform.SetParent(null);
                RestartLayoutAnimation();
            }
        }

        [Button]
        public void UpdateCardPositionsImmediate()
        {
            InitializeCardTransforms();

            var cardCount = _cardViews.Count;
            if (cardCount == 0) return;

            var targetPositions = CalculateCardPositions(cardCount);
            for (int i = 0; i < cardCount; i++)
                if (_cardTransforms[i])
                    _cardTransforms[i].anchoredPosition = targetPositions[i];
        }

        private void InitializeCardTransforms()
        {
            _cardTransforms.Clear();
            foreach (var cv in _cardViews)
                if (cv) _cardTransforms.Add(cv.GetComponent<RectTransform>());
        }
        
        [Button]
        private void RestartLayoutAnimation()
        {
            _layoutCts?.Cancel();
            _layoutCts?.Dispose();
            _layoutCts = new CancellationTokenSource();
            
            AnimateCardPositionsAsync(_layoutCts.Token).Forget();
        }

        private async UniTaskVoid AnimateCardPositionsAsync(CancellationToken token)
        {
            if (_activeSequence != null && _activeSequence.IsActive())
            {
                _activeSequence.Kill();
                _activeSequence = null;
            }
            
            LayoutRebuilder.ForceRebuildLayoutImmediate(_root);
            await UniTask.NextFrame(token);

            int cardCount = _cardViews.Count;
            if (cardCount == 0) 
                return;

            Vector2[] targetPositions = CalculateCardPositions(cardCount);
            Sequence sequence = DOTween.Sequence().SetUpdate(true);
            _activeSequence = sequence;

            for (int i = 0; i < cardCount; i++)
            {
                RectTransform cartRect = _cardTransforms[i];
                if (!cartRect) 
                    continue;

                cartRect.gameObject.SetActive(true);
                cartRect.DOKill();

                sequence.Join(
                    cartRect.DOAnchorPos(targetPositions[i], _animationDuration)
                      .SetEase(_animationEase)
                );
            }
            
            await UniTask.WaitUntil(
                () => sequence == null || !sequence.IsActive() || sequence.IsComplete(),
                cancellationToken: token
            );

            token.ThrowIfCancellationRequested();
        }

        private Vector2[] CalculateCardPositions(int cardCount)
        {
            Vector2[] positions = new Vector2[cardCount];
            if (cardCount == 0) return positions;

            float available = _root.rect.width;
            float spacing = _cardSpacing;

            if (cardCount > 1)
            {
                float maxSpacingByWidth = (available - _cardWidth) / (cardCount - 1);
                spacing = Mathf.Min(spacing, maxSpacingByWidth);
            }

            float totalWidth = (cardCount - 1) * spacing + _cardWidth;
            float startX = -totalWidth * 0.5f + _cardWidth * 0.5f;

            for (int i = 0; i < cardCount; i++)
                positions[i] = new Vector2(startX + i * spacing, _verticalOffset); // << тут Y

            return positions;
        }
    }
}
