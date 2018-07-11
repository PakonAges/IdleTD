using UnityEngine;
using UnityEngine.UI;

public class TowerDetailsView : MonoBehaviour {

    public Text TowerName;
    public Text TowerDetails;

    public Button UpDmg;
    public Button UpRange;
    public Button UpSpeed;

    public Text UpDmgBtnCost;
    public Text UpRangeBtnCost;
    public Text UpSpeedBtnCost;

    private void Start()
    {
        UpDmg.onClick.AddListener(UpgradeDmg);
        UpRange.onClick.AddListener(UpgradeRange);
        UpSpeed.onClick.AddListener(UpgradeSpeed);
    }

    public void UpdateTowerInfo(TowerCode tower)
    {
        TowerName.text = tower.myTower.MyTowerType.ToString();
        TowerDetails.text = "Damage: " + tower.MyDmg + "\n" +
                            "Range: " + tower.MyRange + "\n" +
                            "Speed: " + tower.MyFireSpeed;

        UpDmgBtnCost.text = tower.MyUpgrader.MyDmgUpgradeCost.ToString();
        UpRangeBtnCost.text = tower.MyUpgrader.MyRangeUpgradeCost.ToString();
        UpSpeedBtnCost.text = tower.MyUpgrader.MySpeedUpgradeCost.ToString();
    }

    void UpgradeDmg()
    {
        TowersManager.instance.SelectedTower.GetComponent<TowerCode>().MyUpgrader.UpgradeDmg();
        UpdateTowerInfo(TowersManager.instance.SelectedTower.GetComponent<TowerCode>());
        EventManager.Broadcast(gameEvent.CoinsChanged, new eventArgExtend());
    }

    void UpgradeRange()
    {
        TowersManager.instance.SelectedTower.GetComponent<TowerCode>().MyUpgrader.UpgradeRange();
        UpdateTowerInfo(TowersManager.instance.SelectedTower.GetComponent<TowerCode>());
        EventManager.Broadcast(gameEvent.CoinsChanged, new eventArgExtend());
    }

    void UpgradeSpeed()
    {
        TowersManager.instance.SelectedTower.GetComponent<TowerCode>().MyUpgrader.UpgradeSpeed();
        UpdateTowerInfo(TowersManager.instance.SelectedTower.GetComponent<TowerCode>());
        EventManager.Broadcast(gameEvent.CoinsChanged, new eventArgExtend());
    }
}
