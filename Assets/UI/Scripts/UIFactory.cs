using UnityEngine;
using Zenject;

public class UIFactory
{
    readonly HUDViewModel.Factory _hudFactory;
    readonly BankWindowViewModel.Factory _BankFactory;
    readonly deBugWindowViewModel.Factory _deBugFactory;
    readonly ConfirmExitViewModel.Factory _confirmExitFactory;


    public UIFactory(   HUDViewModel.Factory hudFactory,
                        BankWindowViewModel.Factory bankFactory,
                        deBugWindowViewModel.Factory deBugFactory,
                        ConfirmExitViewModel.Factory confirmExitFactory)
    {
        _hudFactory = hudFactory;
        _BankFactory = bankFactory;
        _deBugFactory = deBugFactory;
        _confirmExitFactory = confirmExitFactory;
    }

    public UIWindow CreateWindow<T>() where T : UIWindow
    {
        if (typeof(T) == typeof(HUDViewModel))
        {
            return _hudFactory.Create();
        }

        if (typeof(T) == typeof(BankWindowViewModel))
        {
            return _BankFactory.Create();
        }

        if (typeof(T) == typeof(deBugWindowViewModel))
        {
            return _deBugFactory.Create();
        }

        if (typeof(T) == typeof(ConfirmExitViewModel))
        {
            return _confirmExitFactory.Create();
        }

        throw new MissingReferenceException("ooops. no such window in UI Manager");
    }
}

