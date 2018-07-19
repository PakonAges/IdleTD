using System.Collections.Generic;
using UnityEngine;

public class Map {

    public int Seed;
    public Dictionary<int,MapSection> MapSections;
    public List<Vector3> PortalList;

    public Map(int seed)
    {
        Seed = seed;
        MapSections = new Dictionary<int, MapSection>();
        PortalList = new List<Vector3>();
    }
}