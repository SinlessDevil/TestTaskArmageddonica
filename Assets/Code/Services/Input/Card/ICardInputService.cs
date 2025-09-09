using System;
using Code.Logic.Grid;
using Code.UI.Game.Cards;
using Code.UI.Game.Cards.PM;
using Code.UI.Game.Cards.View;

namespace Code.Services.Input.Card
{
    public interface ICardInputService
    {
        event Action<CardView, ICardPM, Cell> DroppedOnCell;
        event Action<CardView> ClickPressed;
        event Action<CardView> ClickReleased;
        bool IsDragging { get; }
        bool IsEnabled { get; }
        void Enable(TypeInput typeInput);
        void Disable();
        void PointerEnter(CardView view, ICardPM cardPM);
        void PointerExit(CardView view, ICardPM cardPM);
        void PointerDown(CardView view, ICardPM cardPM);
        void PointerUp(CardView view , ICardPM cardPM);
    }
}