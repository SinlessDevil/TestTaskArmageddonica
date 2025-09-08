using System.Collections.Generic;
using Code.UI.Game.Cards;
using Code.UI.Game.Cards.Holder;
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
        
        private CardHolder _cardHolder; 
        
        private bool _visible = true;
        
        private ICardSelectionPM _cardSelectionPM;

        public void Initialize(ICardSelectionPM cardSelectionPm)
        {
            _cardSelectionPM = cardSelectionPm;
            
            Subscribe();
            
            SetCardHolder(_cardSelectionPM.GetCardHolder());
            SetCards(_cardSelectionPM.GetCards());
        }

        public void Dispose()
        {
            ClearCardsRoot();
            
            Unsubscribe();
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

        private void SetCardHolder(CardHolder cardHolder)
        {
            _cardHolder = cardHolder;
        }
        
        private void SetCards(IReadOnlyList<CardView> cards)
        {
            foreach (CardView cardView in cards)
            {
                if (cardView == null) 
                    continue;
                cardView.transform.SetParent(_cardsRoot, false);
            }
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
            _animator.Close(() => OnClosed(selected));
        }

        private void OnClosed(CardView selected)
        {
            _cardSelectionPM.OnAddCardToHolder(selected);
        }

        private void ClearCardsRoot()
        {
            for (int i = _cardsRoot.childCount - 1; i >= 0; i--)
                Destroy(_cardsRoot.GetChild(i).gameObject);
        }
    }
}