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
    private string _coinsAmount = string.Empty;
    private bool _showSelectionInfo = false;

    //Selection info
    public SelectionData Selection;

    [Inject]
    public void Construct(  SignalBus signalBus,
                            PlayerData playerData)
    {
        _signalBus = signalBus;
        _coins = playerData.Coins.Variable;
    }

    [Binding]
    public bool ShowSelectionInfo
    {
        get { return _showSelectionInfo; }
        set
        {
            if (_showSelectionInfo == value)
            {
                return;
            }
            _showSelectionInfo = value;
            OnPropertyChanged("ShowSelectionInfo");
        }
    }

    [Binding]
    public string CoinsText
    {
        get { return _coinsAmount; }
        set
        {
            if (_coinsAmount == value)
            {
                return;
            }
            _coinsAmount = value;
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
        _uiManager.AddWindowToActiveStack(this);
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
        _uiManager.OpenWindow(UIcollection.ConfirmExit);
    }

    [Binding]
    public void OnDebugBtnPressed()
    {
        _uiManager.OpenWindow(UIcollection.DeBugWindow);
    }

    [Binding]
    public void OnBankBtnPressed()
    {
        _uiManager.OpenWindow(UIcollection.Bank);
    }

    //who will tell me, that tile was selected? Input? NO!
    //Input just clicks
    //Selection Data is the one who decides is if New Tile or not selected!

    //when Tile selected -> Property changed
    //Here I need to take info from SelectionData SO and fill HUD buttons/text
    // -> I show Selection Button in HUD

}
