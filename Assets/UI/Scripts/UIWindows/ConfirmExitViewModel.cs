using UnityEngine;
using UnityWeld.Binding;

[Binding]
public class ConfirmExitViewModel : UIWindow<ConfirmExitViewModel>
{
    public override void OnBackPressed()
    {
        Application.Quit();
    }
}
