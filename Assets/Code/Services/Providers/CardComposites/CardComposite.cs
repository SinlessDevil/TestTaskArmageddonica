using Code.UI.Game.Cards;
using Code.UI.Game.Cards.PM;
using Code.UI.Game.Cards.View;

namespace Code.Services.Providers.CardComposites
{
    public class CardComposite
    {
        public CardView View { get; private set; }
        public ICardPM PM { get; private set; }
        
        public CardComposite(CardView view, ICardPM pm)
        {
            View = view;
            PM = pm;
        }
    }
}