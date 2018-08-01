using UnityEngine;
using Zenject;

public class GameMaster : MonoBehaviour {

    public TowerData _towerData; //temp

    private MapBuilder _mapBuilder;
    private NavMeshCreator _navMeshCreator;
    private MapManager _mapManager;
    private WaveSpawner _waveSpawner;
    private CreepsManager _creepsManager;

    private Tower.Factory _towerFactory;

    [Inject]
    public void Construct(  MapBuilder mapBuilder,
                            NavMeshCreator navMeshCreator,
                            MapManager mapManager,
                            WaveSpawner waveSpawner,
                            CreepsManager creepsManager,
                            Tower.Factory towerFactory)
    {
        _mapBuilder = mapBuilder;
        _navMeshCreator = navMeshCreator;
        _waveSpawner = waveSpawner;
        _mapManager = mapManager;
        _creepsManager = creepsManager;
        _towerFactory = towerFactory;
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
        _creepsManager.StartSpawningCreeps();
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
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            _waveSpawner.RemoveCreep();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            _towerFactory.Create(new Vector3(6.0f,0.5f,-1.5f), _towerData);
        }
    }

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
