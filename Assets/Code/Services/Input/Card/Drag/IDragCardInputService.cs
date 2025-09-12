using System;
using Code.Logic.Grid;
using Code.UI.Game.Cards;
using Code.UI.Game.Cards.PM;
using Code.UI.Game.Cards.View;

namespace Code.Services.Input.Card
{
    public interface IDragCardInputService
    {
        void Enable();
        void Disable();
        void PointerEnter(CardView view);
        void PointerExit(CardView view);
        void PointerDown(CardView view, ICardPM cardPM);
        void PointerUp(CardView view);
    }
}