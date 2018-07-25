/// <summary>
/// Provides Map Generation Data from save
/// </summary>
public class MapDataProvider : IMapDataProvider {

    private readonly MapGenerationData _mapGenerationData;

    public MapDataProvider(SaveLoader saveLoader)
    {
        _mapGenerationData = saveLoader.GetMapData();
    }

    public MapGenerationData GetMapGenerationData()
    {
        return _mapGenerationData;
    }

}
