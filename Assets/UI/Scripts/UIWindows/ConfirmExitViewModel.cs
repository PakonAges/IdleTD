using UnityEngine;

public class ConfirmExitViewModel : UIWindow
{
    public override void OnBackPressed()
    {
        Application.Quit();
    }
}
