using UnityWeld.Binding;

[Binding]
public class ConfirmExitViewModel : UIWindow<ConfirmExitViewModel>
{
    [Binding]
    public void OnExitPressed()
    {
        _uiManager.CloseWindow(this);
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
    }

    [Binding]
    public override void OnBackPressed()
    {
        Close();
    }
}
