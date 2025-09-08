using Code.Services.Factories.UIFactory;
using Code.UI;
using UnityEngine;

namespace Code.Services.Providers.Widgets
{
    public sealed class WidgetFactory : IPoolFactory<Widget>
    {
        private readonly IUIFactory _uiFactory;
        public WidgetFactory(IUIFactory uiFactory) => _uiFactory = uiFactory;

        public Widget Create(Vector3 position, Quaternion rotation, Transform parent = null)
        {
            Widget widget = _uiFactory.CreateWidget(position, rotation);
            
            if (parent) 
                widget.transform.SetParent(parent, false);
            
            return widget;
        }
    }
}