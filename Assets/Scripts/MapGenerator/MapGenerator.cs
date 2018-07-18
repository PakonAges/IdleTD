using System.Collections.Generic;
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
    private readonly DebugSectionBuilder _debugSectionBuilder;
    private SectionPositioner sectionPositioner;

    public MapGenerator(SectionObjectsSpawner sectionObjectsSpawner,
                        MapGenerationData mapGenerationInputata,
                        DebugSectionBuilder debugSectionBuilder)
    {
        _sectionObjectsSpawner = sectionObjectsSpawner;
        _mapGenerationInput = mapGenerationInputata;
        _debugSectionBuilder = debugSectionBuilder;
    }


    /// <summary>
    /// Generates Map Data needed to build Map itself in the builder.
    /// </summary>
    /// <returns>Map data</returns>
    public Map GenerateMap()
    {
        var seed = (int)System.DateTime.Now.Ticks;

        //Use random seed or predefined
        if (_mapGenerationInput.useSeed)
        {
            seed = _mapGenerationInput.Seed;
        }
        
        Random.InitState(seed);

        var GeneratedMap = new Map(seed);

        sectionPositioner = new SectionPositioner(_mapGenerationInput);
        
        //Generate and position empty sections
        for (int i = 1; i < _mapGenerationInput.SectionsAmount + 1; i++)
        {
            var MapSection = GenerateSection(i);
            GeneratedMap.MapSections.Add(i, MapSection);
        }

        //Generate Portals
        var portalGenerator = new PortalGenerator(GeneratedMap, sectionPositioner);

        //Generate Roads in each section
        SectionRoadBuilder roadBuilder = new SectionRoadBuilder(GeneratedMap.MapSections);

        ///Debug mode
        if (deBugDrawSections)
        {
            foreach (KeyValuePair<int, MapSection> section in GeneratedMap.MapSections)
            {
                _debugSectionBuilder.BuildSection(section.Value);
            }
        }
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