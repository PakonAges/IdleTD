using System.Collections.Generic;
using UnityEngine;
using Zenject;

/// <summary>
/// Used to Manipulate Map logic: lock/unlock sections. Add Section to new waypoint path
/// </summary>
public class MapManager {

    public Map Map { get; set; }

    //readonly CreepWayBuilder _creepWayBuilder; //? Where is used?
    //readonly WaypointsSpawner _wayPointSpawner;

    public MapManager(  IMapGenerator mapGenerator
                        //CreepWayBuilder creepWayBuilder,
                        //WaypointsSpawner waypointsSpawner
                        )
    {
        var _mapGenerator = (MapGenerator)mapGenerator;
        Map = _mapGenerator.GeneratedMap;
        //_creepWayBuilder = creepWayBuilder;
        //_wayPointSpawner = waypointsSpawner;
    }

    public void PrepareMapNavigation()
    {
        //check for map state to calculate proper path (locked/unlocked sections)
        LockAllSections(Map.MapSections);
    }

    void LockAllSections(Dictionary<int, MapSection> map)
    {
        foreach (var pair in map)
        {
            if (pair.Key == 1) //HACK
            {
                pair.Value.IsUnlocked = true;
            }
            else
            {
                pair.Value.IsUnlocked = false;
            }
        }
    }


    public void UnlockSection(int Sectionid)
    {
        //_wayPointSpawner.AddNewSection(Sectionid);
    }
}