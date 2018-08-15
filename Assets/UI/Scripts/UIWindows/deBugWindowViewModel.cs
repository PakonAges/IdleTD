using UnityWeld.Binding;

[Binding]
public class deBugWindowViewModel : UIWindow<deBugWindowViewModel>
{
    [Binding]
    public override void OnBackPressed()
    {
        Close();
    }
}