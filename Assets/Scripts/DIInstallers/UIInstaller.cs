using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class UIInstaller : MonoInstaller<UIInstaller>
{
    public List<GameObject> UICollection;

    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<UIManager>().AsSingle();

        foreach (var prefab in UICollection)
        {
            Container.BindFactory<UIwindowEnum, UIWindow, UIWindow.Factory>().FromComponentInNewPrefab(prefab).WhenInjectedInto<CustomUIFactory>();
        }

        Container.BindFactory<UIwindowEnum, UIWindow, UIWindow.Factory>().FromFactory<CustomUIFactory>();
    }
}
