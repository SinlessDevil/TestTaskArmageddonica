using System.Collections.Generic;
using Code.UI.Game.Cards.View;

namespace Code.Services.Providers.CardComposites
{
    public interface ICardCompositeProvider
    {
        List<CardComposite> CreateRandomUnitCards(int count);
        List<CardComposite> CreateMixedTypeCards();
        
        void ReturnCardComposite(CardComposite cardComposite);
        
        void ReturnCardComposite(CardView cardView);
        
        void ReturnCardComposites(List<CardComposite> cardComposites);
    }
}