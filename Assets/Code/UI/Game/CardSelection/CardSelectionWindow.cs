using System.Linq;
using System.Collections.Generic;
using Code.UI.Game.Cards;
using Code.UI.Game.Cards.View;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.Game.CardSelection
{
    public class CardSelectionWindow : MonoBehaviour
    {
        [SerializeField] private Transform _cardsRoot;
        [SerializeField] private Button _rerollButton;
        [SerializeField] private Button _toggleVisibilityButton;
        [SerializeField] private CardSelectionWindowAnimator _animator;
        [Header("Layout Settings")]
        [SerializeField] private float _cardSpacing = 200f;
        [SerializeField] private float _verticalOffset = 0f;
        [SerializeField] private float _cardWidth = 100f;
        
        private List<CardView> _cardViews = new();
        private bool _visible = true;
        
        private ICardSelectionPM _cardSelectionPM;

        public void Initialize(ICardSelectionPM cardSelectionPm)
        {
            _cardSelectionPM = cardSelectionPm;
            
            Subscribe();
            SetCards(_cardSelectionPM.GetCards());
            
            _animator.Initialize();
            _animator.Show();
        }

        public void Dispose()
        {
            Unsubscribe();
            
            Destroy(this.gameObject);
        }

        private void Subscribe()
        {
            _cardSelectionPM.RolledCardsEvent += OnChangedCards;
            _cardSelectionPM.SellectedCardViewEvent += OnSelect;
            
            _rerollButton.onClick.AddListener(OnRerollButtonClick);
            _toggleVisibilityButton.onClick.AddListener(OnToggleVisibilityButtonClick);
        }
        
        private void Unsubscribe()
        {
            _cardSelectionPM.RolledCardsEvent -= OnChangedCards;
            _cardSelectionPM.SellectedCardViewEvent -= OnSelect;
            
            _rerollButton.onClick.RemoveListener(OnRerollButtonClick);
            _toggleVisibilityButton.onClick.RemoveListener(OnToggleVisibilityButtonClick);
        }
        
        private void SetCards(IReadOnlyList<CardView> cards)
        {
            foreach (CardView cardView in cards)
            {
                if (cardView == null) 
                    continue;
                
                cardView.transform.SetParent(_cardsRoot, false);
            }
            _cardViews = cards.ToList();
            LayoutCards(cards);
        }

        private void OnChangedCards()
        {
            ClearCardsRoot();
            SetCards(_cardSelectionPM.GetCards());
        }
        
        private void OnRerollButtonClick()
        {
            _cardSelectionPM.OnRollCards();
        }

        private void OnToggleVisibilityButtonClick()
        {
            _visible = !_visible;
            
            if (_visible)
                _animator.Show();
            else
                _animator.Hide();
        }

        private void OnSelect(CardView view)
        {
            StartCloseSequence(view);
        }

        private void StartCloseSequence(CardView selected)
        {
            OnClosed(selected);
            
            _animator.Close(null);
        }

        private void OnClosed(CardView selected)
        {
            _cardSelectionPM.OnAddCardToHolder(selected);
        }

        private void LayoutCards(IReadOnlyList<CardView> cards)
        {
            var root = _cardsRoot as RectTransform;
            if (root == null)
                return;

            int count = cards.Count(c => c != null);

            if (count == 0)
                return;

            Vector2[] positions = Cards.Extensions.CardLayoutExtensions.CalculateCenteredRowPositions(
                root, count, _cardWidth, _cardSpacing, _verticalOffset);

            int i = 0;
            foreach (CardView card in cards)
            {
                if (card == null) 
                    continue;
                
                RectTransform cardRoot = card.Root;
                if (!cardRoot) 
                    continue;
                
                cardRoot.anchoredPosition = positions[i];
                cardRoot.localScale = Vector3.one;
                i++;
            }
        }

        private void ClearCardsRoot()
        {
            foreach (CardView cardView in _cardViews)
                Destroy(cardView.gameObject);
            
            _cardViews.Clear();
        }
    }
}