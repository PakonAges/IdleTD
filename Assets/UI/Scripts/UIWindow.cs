using System.ComponentModel;
using UnityEngine;
using Zenject;

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

    public abstract void OnBackPressed();

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

public abstract class UIWindow<T> : UIWindow where T : UIWindow<T>
{
    //protected void Open()
    //{
    //    if (gameObject == null)
    //    {
    //        _uiManager.CreateNewWindow<T>();
    //    }
    //    else
    //    {
    //        gameObject.GetComponent<Canvas>().enabled = true;
    //    }

    //    _uiManager.OpenWindow((T)this);
    //}

    protected void Close()
    {
        _uiManager.CloseWindow(this);
    }

    public class Factory : PlaceholderFactory<T>
    {

    }
}