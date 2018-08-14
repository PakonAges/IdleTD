using Zenject;

public class UIInstaller : MonoInstaller<UIInstaller>
{
    public HUDViewModel HUD;
    public BankWindowViewModel BankWindow;
    public ConfirmExitViewModel ExitConfirmWindow;
    public deBugWindowViewModel DebugWindow;

    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<UIManager>().AsSingle().NonLazy();

        Container.BindInstance(HUD).AsSingle().WhenInjectedInto<UIManager>();
        Container.BindInstance(BankWindow).AsSingle().WhenInjectedInto<UIManager>();
        Container.BindInstance(DebugWindow).AsSingle().WhenInjectedInto<UIManager>();
        Container.BindInstance(ExitConfirmWindow).AsSingle().WhenInjectedInto<UIManager>();
    }
}
