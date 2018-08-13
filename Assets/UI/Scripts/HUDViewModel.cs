using System;
using System.ComponentModel;
using UnityEngine;
using UnityWeld.Binding;
using Zenject;

[Binding]
public class HUDViewModel : MonoBehaviour, INotifyPropertyChanged, IDisposable
{
    //Injections
    private IntVariable _coins;
    private SignalBus _signalBus;

    //UI elements
    private string coinsAmount = string.Empty;

    [Inject]
    public void Construct(  SignalBus signalBus,
                            PlayerData playerData)
    {
        _signalBus = signalBus;
        _coins = playerData.Coins.Variable;
    }

    [Binding]
    public string CoinsText
    {
        get { return coinsAmount; }
        set
        {
            if (coinsAmount == value)
            {
                return;
            }
            coinsAmount = value;
            OnPropertyChanged("CoinsText");
        }
    }

    void OnEnable()
    {
        _signalBus.Subscribe<SignalCoinsChanged>(OnCoinsChanged);
    }

    public void Dispose()
    {
        _signalBus.Unsubscribe<SignalCoinsChanged>(OnCoinsChanged);
    }

    void Start()
    {
        RegreshCoinsText();
    }

    private void OnCoinsChanged()
    {
        RegreshCoinsText();
    }

    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public event PropertyChangedEventHandler PropertyChanged;

    private void RegreshCoinsText()
    {
        CoinsText = string.Format("Coins: {0}", _coins.Value.ToString());
    }
}
