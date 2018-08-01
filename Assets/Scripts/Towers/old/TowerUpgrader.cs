using UnityEngine;

public class TowerUpgrader {

    TowerCode _myTower;

    public int MyDmgUpgradeCost { get; set; }
    public int MyRangeUpgradeCost { get; set; }
    public int MySpeedUpgradeCost { get; set; }

    public TowerUpgrader(TowerCode tower)
    {
        _myTower = tower;
    }

    public void UpgradeDmg()
    {
        if (PlayerStats.instance.Coins >= MyDmgUpgradeCost)
        {
            _myTower.MyDmg += 1;
            PlayerStats.instance.Coins -= MyDmgUpgradeCost;
            _myTower.UpdateTowerParamsInSave();
            UpdateCostOfUpgrades();
        } else
            Debug.Log("Not enough Coins to upgrade DMG");

    }

    public void UpgradeRange()
    {
        if (PlayerStats.instance.Coins >= MyRangeUpgradeCost)
        {
            _myTower.MyRange += 0.1f;
            PlayerStats.instance.Coins -= MyRangeUpgradeCost;
            _myTower.UpdateTowerParamsInSave();
            UpdateCostOfUpgrades();
        } else
            Debug.Log("Not enough Coins to upgrade RANGE");
    }

    public void UpgradeSpeed()
    {
        if (PlayerStats.instance.Coins >= MySpeedUpgradeCost)
        {
            _myTower.MyFireSpeed -= 0.01f;

            if (_myTower.MyFireSpeed <= 0) _myTower.MyFireSpeed = 0.01f;

            PlayerStats.instance.Coins -= MySpeedUpgradeCost;
            _myTower.UpdateTowerParamsInSave();
            UpdateCostOfUpgrades();
        } else
            Debug.Log("Not enough Coins to upgrade FIRE SPEED");
    }

    public void UpdateCostOfUpgrades()
    {
        MyDmgUpgradeCost = _myTower.MyDmg * 10;
        MyRangeUpgradeCost = (int)(_myTower.MyRange * 50);
        MySpeedUpgradeCost = (int)(_myTower.MyFireSpeed * 25);
    }
}
