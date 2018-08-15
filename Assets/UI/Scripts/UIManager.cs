using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class UIManager : ITickable
{
    public UIManager.Settings _UI;

    [Serializable]
    public class Settings
    {
        public HUDViewModel HUD;
        public BankWindowViewModel Bank;
        public deBugWindowViewModel DeBugWindow;
        public ConfirmExitViewModel ConfirmExit;
    }

    private Stack<UIWindow> _menuStack = new Stack<UIWindow>();

    //Injections
    readonly UIFactory _uiFactory;

    public UIManager(   UIFactory UIfactory,
                        UIManager.Settings settings)
    {
        _uiFactory = UIfactory;
        _UI = settings;
    }

    public void Tick()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && _menuStack.Count > 0)
        {
            _menuStack.Peek().OnBackPressed();
            Debug.Log("Escape pressed");
        }
    }

    public void OpenWindow(UIWindow window)
    {
        //Hide top Window if it is there
        if (_menuStack.Count > 0)
        {
            if (window.DisableMenusUnderneath)
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

            var topCanvas = window.gameObject.GetComponent<Canvas>();
            var prevCanvas = _menuStack.Peek().gameObject.GetComponent<Canvas>();
            topCanvas.sortingOrder = prevCanvas.sortingOrder + 1;
        }

        //add new window to the Stack
        _menuStack.Push(window);
    }

    public void CreateNewWindow<T>() where T : UIWindow
    {
        _uiFactory.CreateWindow<T>();
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

    public void AddWindowToActiveStack(UIWindow window)
    {
        _menuStack.Push(window);
    }
}