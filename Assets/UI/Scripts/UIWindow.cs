using System.ComponentModel;
using UnityEngine;
using Zenject;

public abstract class UIWindow : MonoBehaviour, INotifyPropertyChanged
{
    //Injection
    protected UIManager _uiManager;

    public bool DestroyWhenClosed = false;
    public bool DisableMenusUnderneath = false;
    protected UIwindowEnum _type;

    [Inject]
    public void Construct(  UIManager uiManager/*,
                            UIwindowEnum uIwindowEnum*/)
    {
        _uiManager = uiManager;
        //_type = uIwindowEnum;
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public virtual void OnBackPressed()
    {
        _uiManager.CloseWindow(this);
    }

    public class Factory : PlaceholderFactory<UIwindowEnum, UIWindow>
    {
    }
}