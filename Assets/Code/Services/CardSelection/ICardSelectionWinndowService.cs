using System;
using Code.UI.Game.Cards.View;
using Code.UI.Game.CardSelection.View;

namespace Code.Services.CardSelection
{
    public interface ICardSelectionWinndowService
    {
        event Action<CardView> SelectedCardEvent;
        event Action ClosedWindowEvent;
        void Open();
        void Close();
        CardSelectionWindow CardSelectionWindow { get; }
    }
}