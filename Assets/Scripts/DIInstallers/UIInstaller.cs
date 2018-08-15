using Zenject;

public class UIInstaller : MonoInstaller<UIInstaller>
{
    public UIManager.Settings UIList;

    public override void InstallBindings()
    {
        Container.BindInstance(UIList);
        Container.BindInterfacesAndSelfTo<UIManager>().AsSingle();

        Container.BindFactory<HUDViewModel, HUDViewModel.Factory>().FromComponentInNewPrefab(UIList.HUD).WhenInjectedInto<UIFactory>();
        Container.BindFactory<BankWindowViewModel, BankWindowViewModel.Factory>().FromComponentInNewPrefab(UIList.Bank).WhenInjectedInto<UIFactory>();
        Container.BindFactory<deBugWindowViewModel, deBugWindowViewModel.Factory>().FromComponentInNewPrefab(UIList.DeBugWindow).WhenInjectedInto<UIFactory>();
        Container.BindFactory<ConfirmExitViewModel, ConfirmExitViewModel.Factory>().FromComponentInNewPrefab(UIList.ConfirmExit).WhenInjectedInto<UIFactory>();

        Container.Bind<UIFactory>().AsSingle();
    }
}
