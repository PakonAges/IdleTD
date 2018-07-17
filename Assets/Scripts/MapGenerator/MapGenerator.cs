using UnityEngine;

public struct MapCell
{
    public int X;
    public int Y;

    public MapCell(int x, int y)
    {
        X = x;
        Y = y;
    }
}

public class GlobalSectionCell
{
    public Vector3 Position;
    public int Xsize;
    public int Ysize;

    public GlobalSectionCell(Vector3 pos, int x, int y)
    {
        Position = pos;
        Xsize = x;
        Ysize = y;
    }
}

public class LocallSectionCell
{
    public MapCell Position;
    public int Xsize;
    public int Ysize;

    public LocallSectionCell(MapCell pos, int x, int y)
    {
        Position = pos;
        Xsize = x;
        Ysize = y;
    }
}

public class MapGenerator {

    private readonly MapBuilder _mapBuilder;
    private readonly SectionObjectsSpawner _sectionObjectsSpawner;

    [Header("Map Setup")]
    public int MapSizeX = 200;
    public int MapSizeY = 200;
    static public int minSectionSize = 4;
    static public int maxSectionSize = 8;
    static public int startSecionSize = 7;
    static public int gapSizeBetweenSections = 1;

    public NewMap Map;
    SectionPositioner sectionPositioner;
    PortalGenerator portalGenerator;

    public MapGenerator(MapBuilder mapBuilder, SectionObjectsSpawner sectionObjectsSpawner)
    {
        _mapBuilder = mapBuilder;
        _sectionObjectsSpawner = sectionObjectsSpawner;
    }

    public NewMap GenerateMap(int sections)
    {
        Map = new NewMap();

        UnityEngine.Random.InitState((int)System.DateTime.Now.Ticks);

        sectionPositioner = new SectionPositioner(MapSizeX, MapSizeY, gapSizeBetweenSections, this);
        

        for (int i = 1; i < sections + 1; i++)
        {
            Map.MapSections.Add(i, GenerateSection(i));
        }
        Debug.Log("Map Created: " + sections + " sections");

        portalGenerator = new PortalGenerator(Map, sectionPositioner);
        Debug.Log("Portals Created: " + portalGenerator.PortalList.Count + " portals total");

        SectionRoadBuilder roadBuilder = new SectionRoadBuilder(Map.MapSections);
        Debug.Log("Section Roads Created");

        _mapBuilder.BuildMap(Map.MapSections, portalGenerator.PortalList);
        _sectionObjectsSpawner.SpawnRocks(Map.MapSections);

        return Map; 
    }

    MapSection GenerateSection(int id)
    {
        GlobalSectionCell section = sectionPositioner.DefineSection(id);

        MapSection NewSection = new MapSection(id, section);

        Debug.Log("Section Builded: " + id + " = [" + section.Xsize + ";" + section.Ysize + "]");

        return NewSection;
    }
}
