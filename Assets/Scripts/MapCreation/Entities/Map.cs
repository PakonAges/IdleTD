using System.Collections.Generic;

public class Map {

    public int Seed;
    public Dictionary<int,MapSection> MapSections;
    public List<Bridge> Bridges;

    public Map(int seed)
    {
        Seed = seed;
        MapSections = new Dictionary<int, MapSection>();
        Bridges = new List<Bridge>();
    }
}