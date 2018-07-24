using UnityEngine;
using Zenject;

public class GameMaster : MonoBehaviour {

    //public MapManager mapManager = new MapManager();
    //public NavMeshSurface navMesh;

    private MapBuilder _mapBuilder;


    [Inject]
    public void Construct(MapBuilder mapBuilder)
    {
        _mapBuilder = mapBuilder;
    }

	void Awake () {

        //mapManager.Map =  GetComponentInChildren<MapGenerator>().GenerateMap();
        //mapManager.Init(GetComponent<WaypointsSpawner>());

        //navMesh.BuildNavMesh();

        //DataManager.instance.LoadData();
        //TimeManager.instance.Init();
        //CreepsManager.instance.Init();
    }

    void Start()
    {
        _mapBuilder.BuildMap();

        //Start spawning Creeps
        //BuildManager.instance.BuildLoadedTowers();
        //UIManager.instance.Init();
        //EventManager.Broadcast(gameEvent.GameLoaded, new eventArgExtend());
    }

    //debug
    //void Update()
    //{        

    //    if (Input.GetKeyDown(KeyCode.G))
    //    {
    //        UnlockSection();
    //    }
    //}

    void OnApplicationQuit()
    {
        //PlayerStats.instance.ExitDate = DateTime.Now;
        //DataManager.instance.SaveData();
    }


    //debug
    //public void UnlockSection()
    //{
    //    i++;
    //    mapManager.UnlockSection(i);
    //}

}
