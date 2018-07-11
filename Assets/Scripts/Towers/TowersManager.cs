using UnityEngine;
using System.Collections.Generic;
using GameData;
using System.Linq;

public class TowersManager : MonoBehaviour {

    static TowersManager towersManager;
    public static TowersManager instance
    {
        get
        {
            if (!towersManager)
            {
                towersManager = FindObjectOfType(typeof(TowersManager)) as TowersManager;

                if (!towersManager)
                {
                    Debug.LogError("There needs to be one active DataManager script on a GameObject in my scene.");
                }
            }

            return towersManager;
        }
    }

    public List<TowerBlueprint> TowersCollection = new List<TowerBlueprint>();
    Dictionary<TowerType, GameObject> TowersDic;

    Dictionary<int,GameObject> ActiveTowers = new Dictionary<int,GameObject>();
    public int CurrentTowerIndex { get; set; }

    public GameObject SelectedTower { get; set; }

    private void Awake()
    {
        PopulateTowertCollection();
        CurrentTowerIndex = 0;
    }


    //It is kind of dumb, and very unsolid.
    void PopulateTowertCollection()
    {
        TowersDic = new Dictionary<TowerType, GameObject>();

        TowersDic.Add(TowerType.Normal, TowersCollection[0].Prefab);
        TowersDic.Add(TowerType.AoE, TowersCollection[1].Prefab);
        TowersDic.Add(TowerType.Lazer, TowersCollection[2].Prefab);
    }

    public GameObject GetTowerGO(TowerType type)
    {
        GameObject tower;

        if (TowersDic.TryGetValue(type, out tower))
        {
            return tower;
        }

        return null;
    }

    public int GetTowerCost(TowerType type)
    {
        for (int i = 0; i < TowersCollection.Count; i++)
        {
            if (TowersCollection[i].Type == type)
                return TowersCollection[i].Cost;
        }

        Debug.LogError("Can't get tower cost, because there is no such tower in Tower Collection:" + type);
        return 0;
    }

    public string GetTowerName(TowerType type)
    {
        for (int i = 0; i < TowersCollection.Count; i++)
        {
            if (TowersCollection[i].Type == type)
                return TowersCollection[i].Name;
        }

        Debug.LogError("Can't get tower Name, because there is no such tower in Tower Collection:" + type);
        return null;
    }

    public void AddTower(GameObject tower)
    {        
        ActiveTowers.Add(CurrentTowerIndex,tower);     
    }

    //public GameObject GetActiveTower(GameObject twr)
    //{
    //    return null;
    //    //return ActiveTowers.Find(obj => obj.gameObject == twr);
    //}
}
