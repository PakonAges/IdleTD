using UnityEngine;
using GameData;
using System.Collections.Generic;

public class BuildManager : MonoBehaviour {

    static BuildManager buildManager;
    public static BuildManager instance
    {
        get
        {
            if (!buildManager)
            {
                buildManager = FindObjectOfType(typeof(BuildManager)) as BuildManager;

                if (!buildManager)
                {
                    Debug.LogError("There needs to be one active BuildManager script on a GameObject in my scene.");
                }
            }

            return buildManager;
        }
    }

    public Dictionary<TowerType,GameObject> TowersCollection;

    public GameObject BuildEffect;



    public void BuildTower(TowerType type, TowerTile MapTile)
    {
        if (PlayerStats.instance.Coins < TowersManager.instance.GetTowerCost(type))
        {
            Debug.Log("Cant build tower, Not Enough Coins");
            return;
        }

        CreateTowerInTile(type, MapTile);

        PlayerStats.instance.PlayerTowers.Add(MapTile.MyTower.GetComponent<TowerCode>().myTower);

        PlayerStats.instance.Coins -= TowersManager.instance.GetTowerCost(type);
        //EventManager.Broadcast(gameEvent.TowerBuild, new eventArgExtend() { /* tower = towerToBuild */ });

        GameObject buildEffect = (GameObject)Instantiate(BuildEffect, MapTile.transform.position, MapTile.transform.rotation);
        Destroy(buildEffect, 2f);   //REFACTOR
    }



    GameObject CreateTowerInTile(TowerType ttype, TowerTile tile)
    {
        GameObject tower = Instantiate(TowersManager.instance.GetTowerGO(ttype), tile.transform.position, Quaternion.identity);
        tower.name = TowersManager.instance.GetTowerName(ttype);
        tower.GetComponent<TowerCode>().SetupTowerSettings(TowersManager.instance.CurrentTowerIndex, ttype, tile);
        TowersManager.instance.AddTower(tower);
        tile.TowerWasBuildHere(tower);

        TowersManager.instance.CurrentTowerIndex++;

        return tower;
    }



    public void BuildLoadedTowers()
    {
        for (int i = 0; i < PlayerStats.instance.PlayerTowers.Count; i++)
        {
            //TowerType type = PlayerStats.instance.PlayerTowers[i].MyTowerType;
            //TowerTile tile = MapManager.instance.GetMapTile(PlayerStats.instance.PlayerTowers[i].MyTowerMapPositionX, PlayerStats.instance.PlayerTowers[i].MyTowerMapPositionY);

            //GameObject Tower = CreateTowerInTile(type, tile);
            //Tower.GetComponent<TowerCode>().MyDmg = PlayerStats.instance.PlayerTowers[i].MyDmg;
            //Tower.GetComponent<TowerCode>().MyRange = PlayerStats.instance.PlayerTowers[i].MyRange;
            //Tower.GetComponent<TowerCode>().MyFireSpeed = PlayerStats.instance.PlayerTowers[i].MyRateOfFire;
            //Tower.GetComponent<TowerCode>().MyUpgrader.UpdateCostOfUpgrades();
        }
    }
}
