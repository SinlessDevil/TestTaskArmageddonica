using System;
using System.Collections.Generic;
using Code.Services.Factories.UIFactory;
using Code.Services.Input.Card;
using Code.Services.Providers;
using Code.UI.Game.Cards;
using Code.UI.Game.Cards.Holder;
using UnityEngine;

namespace Code.UI.Game.CardSelection
{
    public class CardSelectionPM : ICardSelectionPM
    {
        private const int CountCards = 3;
        
        private readonly IPoolProvider<CardView> _cardProvider;
        private readonly IUIFactory _uiFactory;
        private readonly ICardInputService _cardInputService;

        public CardSelectionPM(
            IPoolProvider<CardView> cardProvider,
            IUIFactory uiFactory,
            ICardInputService cardInputService)
        {
            _cardProvider = cardProvider;
            _uiFactory = uiFactory;
            _cardInputService = cardInputService;
        }

        public event Action RolledCardsEvent;
        public event Action<CardView> SellectedCardViewEvent;

        public void Subscribe()
        {
            _cardInputService.ClickReleased += OnSelectCardView;
        }

        public void Unsubscribe()
        {
            _cardInputService.ClickReleased -= OnSelectCardView;
        }

        public CardHolder GetCardHolder()
        {
            return _uiFactory.GameHud.CardHolder;
        }

        public List<CardView> GetCards()
        {
            List<CardView> cards = new List<CardView>(CountCards);
            for (int i = 0; i < CountCards; i++)
                cards.Add(_cardProvider.Get(Vector3.zero, Quaternion.identity,null));
            return cards;
        }

        public void OnRollCards()
        {
            RolledCardsEvent?.Invoke();
        }

        public void OnAddCardToHolder(CardView selected)
        {
            _uiFactory.GameHud.CardHolder.AddCard(selected);
        }
        
        private void OnSelectCardView(CardView view)
        {
            SellectedCardViewEvent?.Invoke(view);
        }
    }
}