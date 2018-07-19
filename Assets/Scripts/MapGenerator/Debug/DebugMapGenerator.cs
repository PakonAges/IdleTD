using System.Collections.Generic;

public class DebugMapGenerator : MapGenerator {

    public bool deBugDrawSections = true;
    public bool deBugDrawPortals = true;

    private readonly MapGenerationData _mapData;
    private readonly DebugSectionBuilder _debugSectionBuilder;

    public DebugMapGenerator(MapGenerationData mapGenerationInput, DebugSectionBuilder debugSectionBuilder) : base(mapGenerationInput)
    {
        _mapData = mapGenerationInput;
        _debugSectionBuilder = debugSectionBuilder;
    }

    protected override void DebugBuilding(Map map)
    {
        if (deBugDrawSections)
        {
            foreach (KeyValuePair<int, MapSection> section in map.MapSections)
            {
                _debugSectionBuilder.BuildSection(section.Value);
            }
        }
        if (deBugDrawPortals)
        {
            _debugSectionBuilder.BuildPortals(map.PortalList);
        }
    }
}
