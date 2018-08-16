using UnityEngine;
using Zenject;

public class GameMaster : MonoBehaviour {

    private MapBuilder _mapBuilder;
    private NavMeshCreator _navMeshCreator;
    private MapManager _mapManager;
    private CreepsManager _creepsManager;
    private TowerBuilder _towerBuilder;

    [Inject]
    public void Construct(  MapBuilder mapBuilder,
                            NavMeshCreator navMeshCreator,
                            MapManager mapManager,
                            CreepsManager creepsManager,
                            TowerBuilder towerBuilder)
    {
        _mapBuilder = mapBuilder;
        _navMeshCreator = navMeshCreator;
        _mapManager = mapManager;
        _creepsManager = creepsManager;
        _towerBuilder = towerBuilder;
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
            _creepsManager.RemoveFirstCreep();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            _towerBuilder.TryBuildTower(new Vector3(6.0f, 0.5f, -1.5f));
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            _towerBuilder.TryBuildTower(new Vector3(8.0f, 0.5f, -1.5f));
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
