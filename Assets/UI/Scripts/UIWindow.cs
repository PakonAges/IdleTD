using System.ComponentModel;
using UnityEngine;
using Zenject;

public abstract class UIWindow : MonoBehaviour, INotifyPropertyChanged
{
    protected UIManager _uiManager;
    public bool hasBeenSpawned = false;

    [Tooltip("Destroy the Game Object when menu is closed (reduces memory usage)")]
    public bool DestroyWhenClosed = false;

    [Tooltip("Disable menus that are under this one in the stack")]
    public bool DisableMenusUnderneath = true;

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }


    public abstract void OnBackPressed();
}

public abstract class UIWindow<T> : UIWindow where T : UIWindow<T>
{
    [Inject]
    public void Construct(UIManager uiManager)
    {
        _uiManager = uiManager;
    }

    protected void Open()
    {
        _uiManager.OpenWindow(this);
        //if (Instance == null)
        //    MenuManager.Instance.CreateInstance<T>();
        //else
        //    Instance.gameObject.SetActive(true);

        //MenuManager.Instance.OpenMenu(Instance);
    }

    protected void Close()
    {
        _uiManager.CloseWindow(this);
        //if (Instance == null)
        //{
        //    Debug.LogErrorFormat("Trying to close menu {0} but Instance is null", typeof(T));
        //    return;
        //}

        //MenuManager.Instance.CloseMenu(Instance);
    }

    public override void OnBackPressed()
    {
        Close();
    }
}