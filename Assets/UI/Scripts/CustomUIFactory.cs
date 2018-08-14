using System.Collections.Generic;
using Zenject;

public class CustomUIFactory : IFactory<UIwindowEnum, UIWindow>
{
    readonly List<UIWindow.Factory> _subFactories;

    public CustomUIFactory(List<UIWindow.Factory> subFactories)
    {
        _subFactories = subFactories;
    }

    public UIWindow Create(UIwindowEnum win)
    {
        return _subFactories[(int)win].Create(win);
    }
}