using System.Collections.ObjectModel;
using Code.Services.Factories.UIFactory;
using Code.UI.Game.Cards;
using UnityEngine;

namespace Code.Services.Providers.Cards
{
    public sealed class CardViewProvider : BasePoolProvider<CardView>
    {
        private const int CountPool = 10;

        private Transform _root;

        private readonly IUIFactory _uiFactory;
        
        public CardViewProvider(
            IPoolFactory<CardView> factory, 
            IUIFactory uiFactory) : base(factory)
        {
            _uiFactory = uiFactory;
        }

        public override void CreatePool()
        {
            _root = CreateRoot();
            CreatePool(CountPool, _root);
            HideCards();
        }

        private void HideCards()
        {
            ReadOnlyCollection<CardView> cardViews = GetPoolSnapshot();
            foreach (CardView cardView in cardViews)
            {
                cardView.Hide();
            }
        }

        public override void CleanupPool()
        {
            base.CleanupPool();
            
            if (_root != null)
                Object.Destroy(_root.gameObject);
        }

        private RectTransform CreateRoot()
        {
            Transform uiRoot = _uiFactory.UIRoot;
            GameObject root = new GameObject(typeof(CardViewProvider).Name, typeof(RectTransform));
            RectTransform rectTransform = root.GetComponent<RectTransform>();
            rectTransform.SetParent(uiRoot, false);
            rectTransform.localScale = Vector3.one;
            rectTransform.anchoredPosition3D = Vector3.zero;
            rectTransform.sizeDelta = Vector2.zero;
            return rectTransform;
        }

        protected override void Activate(CardView item)
        {
            item.Show();
            base.Activate(item);
        }

        protected override void Deactivate(CardView item)
        {
            item.Hide();
            base.Deactivate(item);
        }
    }
}