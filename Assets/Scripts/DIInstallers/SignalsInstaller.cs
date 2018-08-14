using Zenject;

public class SignalsInstaller : Installer<SignalsInstaller>
{
    public override void InstallBindings()
    {
        SignalBusInstaller.Install(Container);

        //Creeps Signals
        Container.DeclareSignal<SignalNewWave>();
        Container.DeclareSignal<SignalCreepDied>();
        Container.DeclareSignal<SignalCreepSpawned>();
        Container.DeclareSignal<SignalCreepsCounterChanged>();

        //Player accounting Signals
        Container.DeclareSignal<SignalCoinsChanged>();
    }
}
