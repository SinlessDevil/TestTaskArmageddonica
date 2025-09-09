using Code.Services.Factories.UIFactory;
using Code.UI.Game.Cards;
using Code.UI.Game.Cards.View;
using UnityEngine;

namespace Code.Services.Providers.Cards
{
    public sealed class CardViewFactory : IPoolFactory<CardView>
    {
        private readonly IUIFactory _uiFactory;
        
        public CardViewFactory(IUIFactory uiFactory) => _uiFactory = uiFactory;

        public CardView Create(Vector3 position, Quaternion rotation, Transform parent = null)
        {
            CardView card = _uiFactory.CreateCardView(position, rotation);
            
            if (parent) 
                card.transform.SetParent(parent, false);
            
            return card;
        }
    }
}