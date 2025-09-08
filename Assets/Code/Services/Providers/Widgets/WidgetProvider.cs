using Code.UI;
using UnityEngine;

namespace Code.Services.Providers.Widgets
{
    public sealed class WidgetProvider : BasePoolProvider<Widget>
    {
        private const int CountPool = 10;

        private Transform _root;
        
        public WidgetProvider(IPoolFactory<Widget> factory) : base(factory) { }
        
        public override void CreatePool()
        {
            _root = CreatRoot();
            CreatePool(CountPool, _root);
        }

        public override void CleanupPool()
        {
            base.CleanupPool();
            Object.Destroy(_root.gameObject);
        }
        
        private Transform CreatRoot()
        {
            GameObject root = new GameObject(typeof(WidgetProvider).Name);
            return root.transform;
        }
    }
}