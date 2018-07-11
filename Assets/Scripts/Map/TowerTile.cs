using GameData;
using UnityEngine;

public class TowerTile : MapTile {

    //Should be ref in TowersManager!!!
    public GameObject MyTower { get; set; }
    public bool HasTower { get; set; }

    public int MyMapPositionX { get; set; }
    public int MyMapPositionY { get; set; }

    void OnMouseDown()
    {
        //If Tile already selected - deselect it
        if (UIManager.instance.IsTowerSelected)
        {
            DeselectTower();
            TowersManager.instance.SelectedTower = null;
            return;
        }

        //If tile has a tower on it -> show details about this tower
        if (HasTower)
        {
            UIManager.instance.IsTowerSelected = true;
            
            TowersManager.instance.SelectedTower = MyTower;
            UIManager.instance.ShowTowerDetailsWindow(MyTower);

            return;
        }

        //Tile is empty, we can build here;
        UIManager.instance.IsTowerSelected = true;
        UIManager.instance.ShopWindow.GetComponent<TowerShop>().SelectedMapTile = this;
        UIManager.instance.ShowShop(true);
    }

    public void TowerWasBuildHere(GameObject tower)
    {
        MyTower = tower;
        HasTower = true;
    }

    void DeselectTower()
    {
        UIManager.instance.HideAll();
        UIManager.instance.IsTowerSelected = false;
        TowersManager.instance.SelectedTower = null;
    }
}
