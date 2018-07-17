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

public class MapGenerator
{
    public bool deBugDrawSections = false;
    public bool deBugDrawPortals = false;

    private readonly SectionObjectsSpawner _sectionObjectsSpawner;
    private readonly MapGenerationData _mapGenerationInput;
    private SectionPositioner sectionPositioner;

    public MapGenerator(SectionObjectsSpawner sectionObjectsSpawner,
                        MapGenerationData mapGenerationInputata)
    {
        _sectionObjectsSpawner = sectionObjectsSpawner;
        _mapGenerationInput = mapGenerationInputata;
    }


    /// <summary>
    /// Generates Map Data needed to build Map itself in the builder.
    /// </summary>
    /// <returns>Map data</returns>
    public Map GenerateMap()
    {
        var seed = (int)System.DateTime.Now.Ticks;

        //Use random seed or predefined?
        if (_mapGenerationInput.useSeed)
        {
            seed = _mapGenerationInput.Seed;
        }
        
        UnityEngine.Random.InitState(seed);

        var GeneratedMap = new Map(seed);

        sectionPositioner = new SectionPositioner(_mapGenerationInput);

        for (int i = 1; i < _mapGenerationInput.SectionsAmount + 1; i++)
        {
            GeneratedMap.MapSections.Add(i, GenerateSection(i));

            if (deBugDrawSections)
            {
                //Draw secton in Editor
            }
        }

        var portalGenerator = new PortalGenerator(GeneratedMap, sectionPositioner);
        if (deBugDrawPortals)
        {
            //Draw Portals
        }

        //Debug.Log("Portals Created: " + portalGenerator.PortalList.Count + " portals total");
        return GeneratedMap;
    }

    MapSection GenerateSection(int id)
    {
        GlobalSectionCell section = sectionPositioner.DefineSection(id);
        MapSection NewSection = new MapSection(id, section);

        //Debug.Log("Section Builded: " + id + " = [" + section.Xsize + ";" + section.Ysize + "]");
        return NewSection;
    }
}
