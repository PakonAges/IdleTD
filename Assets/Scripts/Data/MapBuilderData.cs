using UnityEngine;

[CreateAssetMenu(menuName = "Data/Map Objects Collection")]
public class MapBuilderData : ScriptableObject {

    [Header("Debug Objects")]
    public GameObject WayPoint;

    [Header("Debug Tiles")]
    public GameObject RoadTile;
    public GameObject GroundTile;
    public GameObject Bridge;
    public GameObject SectionPillar;
}