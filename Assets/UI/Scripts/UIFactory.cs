using UnityEngine;

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

    public UIWindow CreateWindow(UIcollection window)
    {
        switch (window)
        {
            case UIcollection.HUD:
            return _hudFactory.Create();
            
            case UIcollection.DeBugWindow:
            return _deBugFactory.Create();
            
            case UIcollection.Bank:
            return _BankFactory.Create();
            
            case UIcollection.ConfirmExit:
            return _confirmExitFactory.Create();

            default:
                throw new MissingReferenceException("ooops. no such window in UI Manager");

        }
    }
}

