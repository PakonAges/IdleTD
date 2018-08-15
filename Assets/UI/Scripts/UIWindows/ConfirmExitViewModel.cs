using UnityEngine;

public class ConfirmExitViewModel : UIWindow<ConfirmExitViewModel>
{
    public override void OnBackPressed()
    {
        Application.Quit();
    }
}
