using System;
using UnityEngine;

public class GameMaster : MonoBehaviour {

    public MapManager mapManager = new MapManager();
    int i = 1; //debug for unlocking sections

	void Awake () {

        mapManager.Map =  GetComponentInChildren<MapGenerator>().GenerateMap(10);
        mapManager.Init(GetComponent<WaypointsSpawner>());

        GetComponentInChildren<UnityEngine.AI.NavMeshSurface>().BuildNavMesh();

        //DataManager.instance.LoadData();
        //TimeManager.instance.Init();
        //CreepsManager.instance.Init();
    }

    void Start()
    {
        //Start spawning Creeps
        //BuildManager.instance.BuildLoadedTowers();
        //UIManager.instance.Init();
        //EventManager.Broadcast(gameEvent.GameLoaded, new eventArgExtend());
    }

    //debug
    void Update()
    {        

        if (Input.GetKeyDown(KeyCode.G))
        {
            UnlockSection();
        }
    }

    void OnApplicationQuit()
    {
        //PlayerStats.instance.ExitDate = DateTime.Now;
        //DataManager.instance.SaveData();
    }


    //debug
    public void UnlockSection()
    {
        i++;
        mapManager.UnlockSection(i);
    }

}
