using System;
using System.Collections.Generic;
using Code.UI.Game.Cards.View;

namespace Code.UI.Game.CardSelection.PM
{
    public interface ICardSelectionPM
    {
        event Action RolledCardsEvent;
        event Action<CardView> SellectedCardViewEvent;
        event Action ClosedWindowEvent;
        void Subscribe();
        void Unsubscribe();
        void Dispose();
        List<CardView> GetCards();
        void OnRollCards();
        void OnAddCardToHolder(CardView selected);
        void OnCloseWindow();
        bool HasFirstOpenWindow();
    }
}