using UnityWeld.Binding;
using Zenject;

[Binding]
public class BankWindowViewModel : UIWindow<BankWindowViewModel>
{
    PlayerAccountant _playerAccountant;

    [Inject]
    public void Construct(PlayerAccountant playerAccountant)
    {
        _playerAccountant = playerAccountant;
    }

    [Binding]
    public override void OnBackPressed()
    {
        Close();
    }

    [Binding]
    public void OnAddCoinsBtnPressed()
    {
        _playerAccountant.AddCoins(100);
    }
}
