using System;
using Code.UI.Game.Cards.View;

namespace Code.Services.Input.Card.Select
{
    public class SelectionCardInputService : ISelectionCardInputService
    {
        public event Action<CardView> ClickPressed;
        
        public event Action<CardView> ClickReleased;
        
        public bool IsEnabled { get; private set; }

        public void Enable()
        {
            if (IsEnabled) 
                return;
            
            IsEnabled = true;
        }

        public void Disable()
        {
            if (!IsEnabled)
                return;
            
            IsEnabled = false;
        }

        public void PointerEnter(CardView view)
        {
            if (!IsEnabled)
                return;

            view.HoverComponent.HighlightOn();
        }

        public void PointerExit(CardView view)
        {
            if (!IsEnabled)
                return;
            
            view.HoverComponent.HighlightOff();
        }

        public void PointerDown(CardView view)
        {
            if (!IsEnabled)
                return;

            ClickPressed?.Invoke(view);
        }

        public void PointerUp(CardView view)
        {
            if (!IsEnabled) 
                return;

            view.HoverComponent.ResetState();
            view.HoverComponent.HoverExit();
            
            ClickReleased?.Invoke(view);
        }
    }
}