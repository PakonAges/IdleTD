using System.Collections.Generic;
using UnityEngine;

public class MapManager {

    public Map Map { get; set; }
    WaypointsSpawner wayPointSpawner;
    public CreepWayBuilder wayBuilder;

    public void Init(WaypointsSpawner wpSpawner)
    {
        //check for map state to calculate proper path (locked/unlocked sections)
        LockAllSections(Map.MapSections);

        wayBuilder = new CreepWayBuilder(Map);
        wayPointSpawner = wpSpawner;
        wayPointSpawner.Init(this);
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
        wayPointSpawner.AddNewSection(Sectionid);
    }
}