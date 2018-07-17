using UnityEngine;

[CreateAssetMenu(menuName = "Data/MapGenerationData")]
public class MapGenerationData : ScriptableObject
{
    [Header("Player Progress Settings")]
    public int SectionsAmount = 3;

    [Header("Global Settings")]
    public int Seed;
    public bool useSeed = false;
    public int MapSizeX = 200;
    public int MapSizeY = 200;
    public int MinSectionSize = 4;
    public int MaxSectionSize = 8;
    public int StartingSecionSize = 7;
    public int GapSize = 1;
}