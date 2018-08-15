using System.ComponentModel;
using UnityEngine;
using UnityWeld.Binding;
using Zenject;


[Binding]
public abstract class UIWindow : MonoBehaviour, INotifyPropertyChanged
{
    //Injection
    protected UIManager _uiManager;

    public bool DestroyWhenClosed = false;
    public bool DisableMenusUnderneath = false;

    [Inject]
    public void Construct(  UIManager uiManager)
    {
        _uiManager = uiManager;
    }

    [Binding]
    public abstract void OnBackPressed();

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

public abstract class UIWindow<T> : UIWindow where T : UIWindow<T>
{
    protected void Open()
    {
        _uiManager.OpenWindow<T>();
    }

    protected void Close()
    {
        _uiManager.CloseWindow(this);
    }

    public override void OnBackPressed()
    {
        Close();
    }

    public class Factory : PlaceholderFactory<T>
    {

    }
}