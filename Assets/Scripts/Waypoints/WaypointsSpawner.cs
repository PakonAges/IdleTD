using GameData;
using System.Collections.Generic;
using UnityEngine;

public class WaypointsSpawner : MonoBehaviour {

    MapManager myManager;

    public GameObject WayPointGO;
    Vector2 Entrance = new Vector2(3, -2);
    int index = 0; //used for naming waypoints

    public void Init(MapManager manager)
    {
        myManager = manager;
        Entrance.x = myManager.Map.MapSections[1].Xsize / 2;
        CreepPath.instance.AddPath(CreateEntrance());
        GenerateCurrentPath();
    }

    public Vector3 CreateEntrance()
    {
        Vector3 wayPointPlace = new Vector3(Entrance.x, 0.0f, -Entrance.y);
        GameObject wayPoint = Instantiate(WayPointGO, wayPointPlace, Quaternion.identity, transform.GetChild(1));
        wayPoint.name = "Spawning Point";
        return wayPoint.transform.position;
    }

    public void GenerateCurrentPath()
    {
        CreepPath.instance.AddPath(GetWpOfSection(1));

        for (int i = 2; i <= myManager.Map.MapSections.Count; i++)
        {
            if (myManager.Map.MapSections[i].IsUnlocked)
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
        return SpawnWpOfSection(myManager.wayBuilder.pathInSections[id]);
    }

    public void AddNewSection(int id)
    {
        int Place = 0;

        List<Vector3> newPath = GetWpOfSection(id);
        //add Section Entrance to The End of the new Path
        newPath.Add(CreateWayPoint(myManager.Map.MapSections[id].EntranceCell, myManager.Map.MapSections[id].PivotPosition));
        //add prev.Section Exit to the End of the End
        Vector3 injectionPoint = FindOtherSideOfThePortal(myManager.Map.MapSections[id].EntranceCell, myManager.Map.MapSections[id].EntranceSide, myManager.Map.MapSections[id].PivotPosition);
        newPath.Add(injectionPoint);

        Place = CreepPath.instance.path.LastIndexOf(injectionPoint);
        

        //Check if this is a corner place, and there might be several same places!!! I need latest one!

        CreepPath.instance.AddPath(Place + 1, newPath);
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
        GameObject wayPoint = Instantiate(WayPointGO, wayPointPlace, Quaternion.identity, transform.GetChild(1));
        wayPoint.name = "Waypoint " + index;
        index++;

        return wayPoint.transform.position;
    }

    //Calibration
    Vector3 CreateWayPoint(MapCell cell, Vector3 pivot)
    {
        Vector3 wayPointPlace = new Vector3(cell.X + pivot.x, 0.0f, -cell.Y + pivot.z);
        GameObject wayPoint = Instantiate(WayPointGO, wayPointPlace, Quaternion.identity, transform.GetChild(1));
        wayPoint.name = "Waypoint " + index;
        index++;

        return wayPoint.transform.position;
    }
}
