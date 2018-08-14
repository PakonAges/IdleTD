using Zenject;

public class UIInstaller : MonoInstaller<UIInstaller>
{
    public UIManager.UIList UIList;

    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<UIManager>().AsSingle().WithArguments(UIList);
        Container.BindFactory<UIWindow, UIWindow.Factory>().FromNewComponentOnNewGameObject();
    }
}
