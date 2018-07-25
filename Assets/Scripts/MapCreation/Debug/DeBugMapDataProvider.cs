/// <summary>
/// Directly provide Map Generation Data from Inspector (binding in Installer)
/// </summary>
public class DeBugMapDataProvider : IMapDataProvider {

    private readonly MapGenerationData _mapGenerationData;

    public DeBugMapDataProvider(MapGenerationData mapGenerationData)
    {
        _mapGenerationData = mapGenerationData;
    }

    public MapGenerationData GetMapGenerationData()
    {
        return _mapGenerationData;
    }

}
