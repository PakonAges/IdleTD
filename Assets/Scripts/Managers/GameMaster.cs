using UnityEngine;
using Zenject;

public class GameMaster : MonoBehaviour {

    private MapBuilder _mapBuilder;
    private NavMeshCreator _navMeshCreator;
    private MapManager _mapManager;


    [Inject]
    public void Construct(  MapBuilder mapBuilder,
                            NavMeshCreator navMeshCreator,
                            MapManager mapManager)
    {
        _mapBuilder = mapBuilder;
        _navMeshCreator = navMeshCreator;
        _mapManager = mapManager;
    }

	void Awake () {

        //mapManager.Init(GetComponent<WaypointsSpawner>());

        //DataManager.instance.LoadData();
        //TimeManager.instance.Init();
        //CreepsManager.instance.Init();
    }

    void Start()
    {
        PrepareLevel();
        //Build Creep Way
        // Build Creep Path
        // Build Waypoints
        // map inited -> 

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

    private void PrepareLevel()
    {
        _mapBuilder.BuildMap();
        _navMeshCreator.GenerateNavMesh();
        _mapManager.PrepareMapNavigation();
    }
}
