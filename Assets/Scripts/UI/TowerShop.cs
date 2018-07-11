using GameData;
using UnityEngine;
using UnityEngine.UI;

public class TowerShop : MonoBehaviour {

    public TowerTile SelectedMapTile { get; set; }

    [Header("Towers cost in Shop UI")]
    public Text FirstTowerCostLine;
    public Text SecondTowerCostLine;
    public Text ThirdTowerCostLine;

    void Start()
    {
        FirstTowerCostLine.text = TowersManager.instance.GetTowerCost(TowerType.Normal).ToString();
        SecondTowerCostLine.text = TowersManager.instance.GetTowerCost(TowerType.AoE).ToString();
        ThirdTowerCostLine.text = TowersManager.instance.GetTowerCost(TowerType.Lazer).ToString();
    }

    public void BuildFirstTower()
    {
        BuildManager.instance.BuildTower(TowerType.Normal, SelectedMapTile);
        UIManager.instance.HideAll();
        UIManager.instance.IsTowerSelected = false;
    }

    public void BuildSecondTower()
    {
        BuildManager.instance.BuildTower(TowerType.AoE, SelectedMapTile);
        UIManager.instance.HideAll();
        UIManager.instance.IsTowerSelected = false;
    }

    public void BuildLazerTower()
    {
        BuildManager.instance.BuildTower(TowerType.Lazer, SelectedMapTile);
        UIManager.instance.HideAll();
        UIManager.instance.IsTowerSelected = false;
    }

}
