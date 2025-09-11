using System;
using System.Collections.Generic;
using System.Linq;
using Code.Services.Factories.UIFactory;
using Code.Services.Input.Card;
using Code.Services.Providers.CardComposites;
using Code.UI.Game.Cards.View;

namespace Code.UI.Game.CardSelection
{
    public class CardSelectionPM : ICardSelectionPM
    {
        private const int CountCards = 3;
        
        private List<CardComposite> _currentCards = new(3);
        
        private readonly ICardCompositeProvider _cardCompositeProvider;
        private readonly IUIFactory _uiFactory;
        private readonly ICardInputService _cardInputService;

        public CardSelectionPM(
            ICardCompositeProvider cardCompositeProvider,
            IUIFactory uiFactory,
            ICardInputService cardInputService)
        {
            _cardCompositeProvider = cardCompositeProvider;
            _uiFactory = uiFactory;
            _cardInputService = cardInputService;
        }

        public event Action RolledCardsEvent;
        public event Action<CardView> SellectedCardViewEvent;
        public event Action ClosedWindowEvent;

        public void Dispose() => ReturnCurrentCards();
        
        public void Subscribe() => _cardInputService.ClickReleased += OnSelectCardView;

        public void Unsubscribe() => _cardInputService.ClickReleased -= OnSelectCardView;

        public List<CardView> GetCards()
        {
            _currentCards = _cardCompositeProvider.CreateRandomUnitCards(CountCards);
            return _currentCards.Select(cardComposite => cardComposite.View).ToList();
        }

        public void OnRollCards()
        {
            ReturnCurrentCards();
            
            RolledCardsEvent?.Invoke();
        }

        public void OnAddCardToHolder(CardView selected)
        {
            _uiFactory.GameHud.CardHolder.AddCard(selected);
            
            CardComposite cardToRemove = _currentCards.FirstOrDefault(card => card.View == selected);
            if (cardToRemove != null) 
                _currentCards.Remove(cardToRemove);
        }

        private void OnSelectCardView(CardView view) => SellectedCardViewEvent?.Invoke(view);

        public void OnCloseWindow() => ClosedWindowEvent?.Invoke();

        private void ReturnCurrentCards()
        {
            if (_currentCards.Count <= 0) 
                return;
            
            _cardCompositeProvider.ReturnCardComposites(_currentCards);
            _currentCards.Clear();
        }
    }
}