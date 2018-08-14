using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class UIManager : ITickable
{
    public readonly UIList UI;
    readonly UIWindow.Factory _uiFactory;

    [Serializable]
    public class UIList
    {
        public HUDViewModel HUD;
        public BankWindowViewModel BankWindow;
        public ConfirmExitViewModel ExitConfirmWindow;
        public deBugWindowViewModel DebugWindow;
    }

    private Stack<UIWindow> _menuStack = new Stack<UIWindow>();

    public UIManager(   UIList uIList,
                        UIWindow.Factory UIfactory)
    {
        UI = uIList;
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

    public void AddHUDtoStack()
    {
        _menuStack.Push(UI.HUD);
    }

    //public void CreateInstance<T>() where T: UIWindow
    //{
    //    var prefab = GetPrefab<T>();
    //    GameObject.Instantiate(prefab);
    //}

    //private T GetPrefab<T>() where T : UIWindow
    //{
    //    // Get prefab dynamically, based on public fields set from Unity
    //    // You can use private fields with SerializeField attribute too
    //    var fields = this.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
    //    foreach (var field in fields)
    //    {
    //        var prefab = field.GetValue(this) as T;
    //        if (prefab != null)
    //        {
    //            return prefab;
    //        }
    //    }

    //    throw new MissingReferenceException("Prefab not found for type " + typeof(T));
    //}

    public void OpenWindow(UIWindow window)
    {
        if (!window.hasBeenSpawned)
        {
            _uiFactory.Create(window);
            window.hasBeenSpawned = true;
        }

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
