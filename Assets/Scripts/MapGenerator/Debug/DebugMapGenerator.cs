using System.Collections.Generic;

public class DebugMapGenerator : MapGenerator {

    public bool deBugDrawIndexMap = false;
    public bool deBugDrawSections = false;
    public bool deBugDrawBridges = false;

    private readonly MapGenerationData _mapData;
    private readonly DebugSectionBuilder _debugSectionBuilder;

    public DebugMapGenerator(MapGenerationData mapGenerationInput, DebugSectionBuilder debugSectionBuilder) : base(mapGenerationInput)
    {
        _mapData = mapGenerationInput;
        _debugSectionBuilder = debugSectionBuilder;
    }

    protected override void DebugBuilding(Map map, int[,] localMap)
    {
        if (deBugDrawIndexMap)
        {
            _debugSectionBuilder.BuildLocalMap(localMap);
        }

        if (deBugDrawSections)
        {
            foreach (KeyValuePair<int, MapSection> section in map.MapSections)
            {
                _debugSectionBuilder.BuildSection(section.Value);
            }
        }
        if (deBugDrawBridges)
        {
            _debugSectionBuilder.BuildPortals(map.Bridges);
        }
    }
}
