using System;
using System.Collections.Generic;
using Code.UI.Game.Cards;
using Code.UI.Game.Cards.Holder;
using Code.UI.Game.Cards.View;

namespace Code.UI.Game.CardSelection
{
    public interface ICardSelectionPM
    {
        event Action RolledCardsEvent;
        event Action<CardView> SellectedCardViewEvent;
        void Subscribe();
        void Unsubscribe();
        List<CardView> GetCards();
        void OnRollCards();
        void OnAddCardToHolder(CardView selected);
    }
}