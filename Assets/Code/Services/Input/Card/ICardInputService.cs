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
        void Enable();
        void Disable();
        void BeginDrag(CardView view);
        void CancelDrag();
    }
}