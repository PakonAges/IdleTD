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
    //Top Left Coord of the Section in the world
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

public class MapGenerator : IMapGenerator
{
    Map generatedMap;
    public Map GeneratedMap
    {
        get
        {
            if (generatedMap == null)
            {
                generatedMap = GenerateMap();
            }

            return generatedMap;
        }
        set
        {
            Debug.Log("Something is Trying to set Generated Map from outside!");
        }
    }

    private readonly MapGenerationData _mapGenerationInput;
    private SectionPositioner sectionPositioner;

    public MapGenerator(IMapDataProvider dataProvider)
    {
        //var _dataProvider = (MapDataProvider)dataProvider;

        _mapGenerationInput = dataProvider.GetMapGenerationData();
        generatedMap = GenerateMap();
    }

    /// <summary>
    /// Generates Map Data needed to build Map itself in the builder.
    /// </summary>
    /// <returns>Map data</returns>
    public Map GenerateMap()
    {
        var GeneratedMap = new Map(GenerateSeed());
        sectionPositioner = new SectionPositioner(_mapGenerationInput);
        
        //Generate and position empty sections
        for (int i = 1; i < _mapGenerationInput.SectionsAmount + 1; i++)
        {
            var MapSection = GenerateSection(i);
            GeneratedMap.MapSections.Add(i, MapSection);
        }

        //Generate Portals
        new BridgeGenerator(GeneratedMap, sectionPositioner);

        //Generate Roads in each section
        new SectionRoadGenerator(GeneratedMap.MapSections);

        return GeneratedMap;
    }

    MapSection GenerateSection(int id)
    {
        GlobalSectionCell section = sectionPositioner.DefineSection(id);
        MapSection NewSection = new MapSection(id, section);

        return NewSection;
    }

    //Use predefined seed or generate new one
    int GenerateSeed()
    {
        var seed = (int)System.DateTime.Now.Ticks;
        if (_mapGenerationInput.useSeed)
        {
            seed = _mapGenerationInput.Seed;
        }

        Random.InitState(seed);
        return seed;
    }
}