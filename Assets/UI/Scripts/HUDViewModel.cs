using System.ComponentModel;
using UnityEngine;
using UnityWeld.Binding;
using Zenject;

[Binding]
public class HUDViewModel : MonoBehaviour, INotifyPropertyChanged
{
    //Injections
    private PlayerData _playerData;
    private SignalBus _signalBus;

    //UI elements
    private string coinsAmount = string.Empty;

    [Inject]
    public void Construct(  SignalBus signalBus,
                            PlayerData playerData)
    {
        _signalBus = signalBus;
        _playerData = playerData;
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
        _signalBus.Subscribe<SignalCreepDied>(OnCreepDied);
    }

    void OnDisable()
    {
        _signalBus.Unsubscribe<SignalCreepDied>(OnCreepDied);
    }

    void Start()
    {
        SetCoinsText();
    }

    private void OnCreepDied(SignalCreepDied args)
    {
        SetCoinsText();
    }

    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public event PropertyChangedEventHandler PropertyChanged;

    private void SetCoinsText()
    {
        CoinsText = string.Format("Coins: {0}", _playerData.Coins.Variable.Value.ToString());
    }
}
