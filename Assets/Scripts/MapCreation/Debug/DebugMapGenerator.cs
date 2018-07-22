using System.Collections.Generic;
using UnityEngine;

public class DebugMapGenerator : IMapGenerator {

    public bool DeBugDrawSections = true;
    public bool DeBugDrawBridges = true;
    public bool DeBugDrawIndexMap = false;

    private readonly MapGenerationData _mapData;
    private readonly DebugSectionBuilder _debugSectionBuilder;
    private SectionPositioner sectionPositioner;

    public DebugMapGenerator(   MapGenerationData mapGenerationInput,
                                DebugSectionBuilder debugSectionBuilder)
    {
        _mapData = mapGenerationInput;
        _debugSectionBuilder = debugSectionBuilder;
    }

    public Map GenerateMap()
    {
        var GeneratedMap = new Map(GenerateSeed());
        sectionPositioner = new SectionPositioner(_mapData);

        //Generate and position empty sections
        for (int i = 1; i < _mapData.SectionsAmount + 1; i++)
        {
            var MapSection = GenerateSection(i);
            GeneratedMap.MapSections.Add(i, MapSection);
        }

        //Generate Portals
        new BridgeGenerator(GeneratedMap, sectionPositioner);

        //Generate Roads in each section
        new SectionRoadGenerator(GeneratedMap.MapSections);

        DebugBuilding(GeneratedMap, sectionPositioner.localMap);

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
        if (_mapData.useSeed)
        {
            seed = _mapData.Seed;
        }

        Random.InitState(seed);
        return seed;
    }

    void DebugBuilding(Map map, int[,] localMap)
    {
        if (DeBugDrawIndexMap)
        {
            _debugSectionBuilder.BuildLocalMap(localMap);
        }

        if (DeBugDrawSections)
        {
            foreach (KeyValuePair<int, MapSection> section in map.MapSections)
            {
                _debugSectionBuilder.BuildSection(section.Value);
            }
        }
        if (DeBugDrawBridges)
        {
            _debugSectionBuilder.BuildPortals(map.Bridges);
        }
    }

}
