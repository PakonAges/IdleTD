using GameData;
using System.Collections.Generic;
using UnityEngine;

public class WaypointsSpawner {

    private readonly MapBuilderData _mapBuilderData;

    private readonly MapManager _mapManager;
    private readonly CreepPath _creepPath;
    private readonly CreepWayBuilder _creepWayBuilder;

    Vector2 Entrance = new Vector2(3, -2);
    int index = 0; //used for naming waypoints

    public WaypointsSpawner(    MapManager mapManager,
                                CreepPath creepPath,
                                CreepWayBuilder creepWayBuilder,
                                MapBuilderData mapBuilderData)
    {
        _mapManager = mapManager;
        _creepPath = creepPath;
        _creepWayBuilder = creepWayBuilder;
        _mapBuilderData = mapBuilderData; // Just for a Waypoint prefab? hmm

        Entrance.x = _mapManager.Map.MapSections[1].Xsize / 2;
        _creepPath.AddPath(CreateEntrance());
        GenerateCurrentPath();
    }

    public Vector3 CreateEntrance()
    {
        Vector3 wayPointPlace = new Vector3(Entrance.x, 0.0f, -Entrance.y);
        GameObject wayPoint = GameObject.Instantiate(_mapBuilderData.WayPoint, wayPointPlace, Quaternion.identity);
        wayPoint.name = "Spawning Point";
        return wayPoint.transform.position;
    }

    public void GenerateCurrentPath()
    {
        _creepPath.AddPath(GetWpOfSection(1));

        for (int i = 2; i <= _mapManager.Map.MapSections.Count; i++)
        {
            if (_mapManager.Map.MapSections[i].IsUnlocked)
            {
                AddNewSection(i);
            }
        }
    }

    public List<Vector3> SpawnWpOfSection(List<Vector2> list)
    {
        List<Vector3> WayPointList = new List<Vector3>();

        foreach (var wayPoint in list)
        {
            WayPointList.Add(CreateWayPoint(wayPoint.x, wayPoint.y));
        }

        return WayPointList;
    }

    List<Vector3> GetWpOfSection(int id)
    {
        return SpawnWpOfSection(_creepWayBuilder.PathInSections[id]);
    }

    public void AddNewSection(int id)
    {
        int Place = 0;

        List<Vector3> newPath = GetWpOfSection(id);
        //add Section Entrance to The End of the new Path
        newPath.Add(CreateWayPoint(_mapManager.Map.MapSections[id].EntranceCell, _mapManager.Map.MapSections[id].PivotPosition));
        //add prev.Section Exit to the End of the End
        Vector3 injectionPoint = FindOtherSideOfThePortal(_mapManager.Map.MapSections[id].EntranceCell, _mapManager.Map.MapSections[id].EntranceSide, _mapManager.Map.MapSections[id].PivotPosition);
        newPath.Add(injectionPoint);

        Place = _creepPath.path.LastIndexOf(injectionPoint);
        

        //Check if this is a corner place, and there might be several same places!!! I need latest one!

        _creepPath.AddPath(Place + 1, newPath);
    }

    Vector3 FindOtherSideOfThePortal(MapCell Entrance, Side side, Vector3 pivot)
    {
        switch (side)
        {
            case Side.None:
                return Vector3.zero;

            case Side.Top:
                return CreateWayPoint(Entrance.X + pivot.x, Entrance.Y - 2 - pivot.z);

            case Side.Right:
                return CreateWayPoint(Entrance.X + 2 + pivot.x, Entrance.Y - pivot.z);

            case Side.Bot:
                return CreateWayPoint(Entrance.X + pivot.x, Entrance.Y + 2 - pivot.z);

            case Side.Left:
                return CreateWayPoint(Entrance.X - 2 + pivot.x, Entrance.Y - pivot.z);

            default:
                return Vector3.zero;
        }
    }

    Vector3 CreateWayPoint(float x, float y)
    {
        Vector3 wayPointPlace = new Vector3(x, 0.0f, -y);
        GameObject wayPoint = GameObject.Instantiate(_mapBuilderData.WayPoint, wayPointPlace, Quaternion.identity);
        wayPoint.name = "Waypoint " + index;
        index++;

        return wayPoint.transform.position;
    }

    //Calibration
    Vector3 CreateWayPoint(MapCell cell, Vector3 pivot)
    {
        Vector3 wayPointPlace = new Vector3(cell.X + pivot.x, 0.0f, -cell.Y + pivot.z);
        GameObject wayPoint = GameObject.Instantiate(_mapBuilderData.WayPoint, wayPointPlace, Quaternion.identity);
        wayPoint.name = "Waypoint " + index;
        index++;

        return wayPoint.transform.position;
    }
}
