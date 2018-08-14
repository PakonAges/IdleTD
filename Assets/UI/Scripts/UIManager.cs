using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class UIManager : ITickable
{
    //Injections
    readonly UIWindow.Factory _uiFactory;

    private Stack<UIWindow> _menuStack = new Stack<UIWindow>();
    //private List<GameObject> _spawnedWindows;

    public UIManager(UIWindow.Factory UIfactory)
    {
        _uiFactory = UIfactory;
    }

    public void Tick()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && _menuStack.Count > 0)
        {
            _menuStack.Peek().OnBackPressed();
            Debug.Log("Escape pressed");
        }
    }

    //public void AddHUDtoStack()
    //{
    //    _menuStack.Push(UI.HUD);
    //}

    public void OpenWindow(UIwindowEnum windowType)
    {
        var newWindow = _uiFactory.Create(windowType);
        //_spawnedWindows.Add(newWindow.gameObject);

        //Hide top Window if it is there
        if (_menuStack.Count > 0)
        {
            if (newWindow.DisableMenusUnderneath)
            {
                foreach (var win in _menuStack)
                {
                    win.gameObject.GetComponent<Canvas>().enabled = false;

                    if (win.DisableMenusUnderneath)
                    {
                        break;

                    }
                }
            }

            var topCanvas = newWindow.gameObject.GetComponent<Canvas>();
            var prevCanvas = _menuStack.Peek().gameObject.GetComponent<Canvas>();
            topCanvas.sortingOrder = prevCanvas.sortingOrder + 1;
        }

        //add new window to the Stack
        _menuStack.Push(newWindow);
    }

    public void CloseWindow(UIWindow window)
    {
        if (_menuStack.Count == 0)
        {
            Debug.LogErrorFormat(window, "{0} cannot be closed because menu stack is empty", window.GetType());
            return;
        }

        if (_menuStack.Peek() != window)
        {
            Debug.LogErrorFormat(window, "{0} cannot be closed because it is not on top of stack", window.GetType());
            return;
        }

        CloseTopMenu();
    }

    private void CloseTopMenu()
    {
        var window = _menuStack.Pop();

        if (window.DestroyWhenClosed)
        {
            GameObject.Destroy(window.gameObject); //Or destroy another way? like a pro with DI
        }
        else
        {
            window.gameObject.GetComponent<Canvas>().enabled = false;
        }

        // Re-activate top menu
        // If a re-activated menu is an overlay we need to activate the menu under it
        foreach (var win in _menuStack)
        {
            win.gameObject.GetComponent<Canvas>().enabled = true;

            if (win.DisableMenusUnderneath)
                break;
        }

    }
}

//So-so solution: order is important and should be the same as in the List in the UIInstaller
public enum UIwindowEnum
{
    HUD = 0,
    Debug,
    Bank,
    ConfirmExit
}