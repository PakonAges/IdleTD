using System;
using Zenject;

public class PlayerAccountant: IInitializable, IDisposable {

    //Injections
    private IntVariable _coins;
    private SignalBus _signalBus;

    public PlayerAccountant(    PlayerData playerData,
                                SignalBus signalBus)
    {
        _coins = playerData.Coins.Variable;
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
        AddCoins(args.Creep.CreepData.Reward);
    }

    private void AddCoins(int amount)
    {
        _coins.Value += amount;
        _signalBus.Fire(new SignalCoinsChanged());
    }

    private bool TryRemoveCoins(int amount)
    {
        if (_coins.Value < amount)
        {
            return false;
        }
        else
        {
            _coins.Value -= amount;
            _signalBus.Fire(new SignalCoinsChanged());
            return true;
        }
    }
}