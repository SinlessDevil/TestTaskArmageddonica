using Code.UI.Game.Cards;
using UnityEngine;

namespace Code.Services.Providers.Cards
{
    public sealed class CardViewProvider : BasePoolProvider<CardView>
    {
        private const int CountPool = 10;

        private Transform _root;

        public CardViewProvider(CardViewFactory factory) : base(factory) { }

        public override void CreatePool()
        {
            _root = CreateRoot();
            CreatePool(CountPool, _root);
        }

        public override void CleanupPool()
        {
            base.CleanupPool();
            
            if (_root != null)
                Object.Destroy(_root.gameObject);
        }

        private Transform CreateRoot()
        {
            GameObject root = new GameObject(typeof(CardViewProvider).Name);
            return root.transform;
        }

        protected override void Activate(CardView item)
        {
            base.Activate(item);
            item.HoverComponent?.Exit();
        }

        protected override void Deactivate(CardView item)
        {
            base.Deactivate(item);
        }
    }
}