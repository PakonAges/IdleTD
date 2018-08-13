using System;
using Zenject;

public class PlayerAccountant: IInitializable, IDisposable {

    //Injections
    private PlayerData _playerData;
    private SignalBus _signalBus;

    public PlayerAccountant(    PlayerData playerData,
                                SignalBus signalBus)
    {
        _playerData = playerData;
        _signalBus = signalBus;
    }

    public void Initialize()
    {
        _signalBus.Subscribe<SignalCreepDied>(OnCreepDied);
    }

    public void Dispose()
    {
        _signalBus.Unsubscribe<SignalCreepDied>(OnCreepDied);
    }

    private void OnCreepDied(SignalCreepDied args)
    {
        _playerData.Coins.Variable.Value += args.Creep.CreepData.Reward;
    }
}