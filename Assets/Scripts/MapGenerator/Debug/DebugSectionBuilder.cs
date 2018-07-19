using UnityEngine;
using GameData;
using System.Collections.Generic;

public class DebugSectionBuilder : MonoBehaviour {

    public GameObject EmptyTile;
    public GameObject RoadTile;
    public GameObject BridgeTile;
    public GameObject GroundTile;
    public GameObject EntranceTile;
    public GameObject ExitTile;
    public GameObject ErrorTile;

    private Transform sectionTransform;
    private Vector3 sectionPivot = new Vector3();

    public void BuildSection(MapSection section)
    {
        //Create empty holder for the section tiles
        var sectionGO = new GameObject
        {
            name = "Section: " + section.SectionId
        };
        sectionTransform = sectionGO.transform;
        sectionPivot = section.PivotPosition;

        //Spawn Entrance Tile
        BuildTile(section.EntranceCell.X, section.EntranceCell.Y, TileType.Entrance);

        //Spawn Exit tiles
        if (section.DoesHaveAnExit)
        {
            foreach (var exit in section.ExitCells)
            {
                BuildTile(exit.X, exit.Y, TileType.Exit);
            }
        }

        for (int j = 0; j < section.SectionTopography.GetLength(1); j++)
        {
            for (int i = 0; i < section.SectionTopography.GetLength(0); i++)
            {
                BuildTile(i, j, section.SectionTopography[i, j]);
            }
        }

        Debug.Log("Section Builded: " + section.SectionId);
    }

    public void BuildPortals(List<Vector3> portalList)
    {
        foreach (var place in portalList)
        {
            var Portal = Instantiate(BridgeTile, place, Quaternion.identity);
            Portal.name = "Portal";
        }
    }

    GameObject BuildTile (int x, int y, TileType type)
    {
        var pos = new Vector3();
        pos = sectionPivot;
        pos.x += x;
        pos.z -= y;

        GameObject TileToBuild;

        switch (type)
        {
            case TileType.Empty:
            TileToBuild = EmptyTile;
            break;

            case TileType.Ground:
            TileToBuild = GroundTile;
            break;

            case TileType.Road:
            TileToBuild = RoadTile;
            break;

            case TileType.Bridge:
            pos.y += 1;
            TileToBuild = BridgeTile;
            break;

            case TileType.Entrance:
            pos.y += 1;
            TileToBuild = EntranceTile;
            break;

            case TileType.Exit:
            pos.y += 1;
            TileToBuild = ExitTile;
            break;

            default:
            TileToBuild = ErrorTile;
            break;
        }

        var Tile = Instantiate(TileToBuild, pos, Quaternion.identity);
        Tile.name = type.ToString() + " [" + x + ";" + y + "]";
        Tile.transform.SetParent(sectionTransform);

        return Tile;
    }
}
