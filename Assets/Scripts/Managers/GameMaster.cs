using UnityEngine;
using Zenject;

public class GameMaster : MonoBehaviour {

    //public MapManager mapManager = new MapManager();

    private MapBuilder _mapBuilder;
    private NavMeshCreator _navMeshCreator;


    [Inject]
    public void Construct(  MapBuilder mapBuilder,
                            NavMeshCreator navMeshCreator)
    {
        _mapBuilder = mapBuilder;
        _navMeshCreator = navMeshCreator;
    }

	void Awake () {

        //mapManager.Init(GetComponent<WaypointsSpawner>());

        //navMesh.BuildNavMesh();

        //DataManager.instance.LoadData();
        //TimeManager.instance.Init();
        //CreepsManager.instance.Init();
    }

    void Start()
    {
        _mapBuilder.BuildMap();
        _navMeshCreator.GenerateNavMesh();

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
