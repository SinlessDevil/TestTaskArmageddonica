using System;
using Code.Logic.Grid;
using Code.UI.Game.Cards;

namespace Code.Services.Input.Card
{
    public interface ICardInputService
    {
        event Action<CardView, Cell> DroppedOnCell;
        bool IsDragging { get; }
        bool IsEnabled { get; }
        void Enable(TypeInput typeInput);
        void Disable();
        void PointerEnter(CardView view);
        void PointerExit(CardView view);
        void PointerDown(CardView view);
        void PointerUp(CardView view);
    }
}