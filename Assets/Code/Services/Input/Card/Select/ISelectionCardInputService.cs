using System;
using Code.UI.Game.Cards.View;

namespace Code.Services.Input.Card.Select
{
    public interface ISelectionCardInputService
    {
        event Action<CardView> ClickPressed;
        event Action<CardView> ClickReleased;
        bool IsEnabled { get; }
        void Enable();
        void Disable();
        void PointerEnter(CardView view);
        void PointerExit(CardView view);
        void PointerDown(CardView view);
        void PointerUp(CardView view);
    }
}