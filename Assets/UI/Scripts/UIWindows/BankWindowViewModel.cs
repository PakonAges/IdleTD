using UnityWeld.Binding;

[Binding]
public class BankWindowViewModel : UIWindow<BankWindowViewModel>
{
    [Binding]
    public override void OnBackPressed()
    {
        Close();
    }
}
