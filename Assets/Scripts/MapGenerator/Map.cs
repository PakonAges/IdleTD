using System.Collections.Generic;

public class Map {

    public int Seed;
    public Dictionary<int,MapSection> MapSections;

    public Map(int seed)
    {
        Seed = seed;
        MapSections = new Dictionary<int, MapSection>();
    }
}