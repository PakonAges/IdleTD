using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class UIFactory
{
    readonly DiContainer _container;

    readonly HUDViewModel.Factory _hudFactory;
    readonly BankWindowViewModel.Factory _BankFactory;
    readonly deBugWindowViewModel.Factory _deBugFactory;
    readonly ConfirmExitViewModel.Factory _confirmExitFactory;


    public UIFactory(   DiContainer container,
                        HUDViewModel.Factory hudFactory,
                        BankWindowViewModel.Factory bankFactory,
                        deBugWindowViewModel.Factory deBugFactory,
                        ConfirmExitViewModel.Factory confirmExitFactory)
    {
        _container = container;
        _hudFactory = hudFactory;
        _BankFactory = bankFactory;
        _deBugFactory = deBugFactory;
        _confirmExitFactory = confirmExitFactory;
    }

    public UIWindow CreateWindow<T>() where T : UIWindow
    {
        return _BankFactory.Create();
    }
}

