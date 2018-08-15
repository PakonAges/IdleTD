using System;
using UnityWeld.Binding;
using Zenject;

[Binding]
public class HUDViewModel : UIWindow<HUDViewModel>, IDisposable
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
        //_uiManager.AddHUDtoStack();
        RefreshCoinsText();
    }

    private void OnCoinsChanged()
    {
        RefreshCoinsText();
    }

    private void RefreshCoinsText()
    {
        CoinsText = string.Format("Coins: {0}", _coins.Value.ToString());
    }

    public override void OnBackPressed()
    {
        _uiManager.OpenWindow<ConfirmExitViewModel>();
    }

    [Binding]
    public void OnDebugBtnPressed()
    {
        _uiManager.OpenWindow<deBugWindowViewModel>();
    }

    [Binding]
    public void OnBankBtnPressed()
    {
        _uiManager.OpenWindow<BankWindowViewModel>();
    }
}
