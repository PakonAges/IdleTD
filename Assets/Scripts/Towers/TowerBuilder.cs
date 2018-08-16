using UnityEngine;

public class TowerBuilder
{
    readonly TowerData _towerData;
    readonly Tower.Factory _towerFactory;
    readonly PlayerAccountant _playerAccountant;
    readonly UIManager _uIManager;

    public TowerBuilder(    TowerData towerData,
                            Tower.Factory factory,
                            PlayerAccountant playerAccountant,
                            UIManager uIManager)
    {
        _towerData = towerData;
        _towerFactory = factory;
        _playerAccountant = playerAccountant;
        _uIManager = uIManager;
    }

    public void TryBuildTower(Vector3 position)
    {
        if (_playerAccountant.TryRemoveCoins(_towerData.BuildCost))
        {
            _towerFactory.Create(position, _towerData);
        }
        else
        {
            _uIManager.OpenWindow(UIcollection.Bank);
        }
    }
}
