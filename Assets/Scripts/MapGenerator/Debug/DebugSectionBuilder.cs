using UnityEngine;
using GameData;

public class DebugSectionBuilder : MonoBehaviour {

    public GameObject Road;
    public GameObject Ground;
    public GameObject Entrance;
    public GameObject Exit;

    public void BuildSection(MapSection section)
    {
        var sectionGO = Instantiate(new GameObject());
        sectionGO.name = "Section: " + section.SectionId;

        for (int i = 0; i < section.SectionTopography.GetLength(0); i++)
        {
            for (int j = 0; j < section.SectionTopography.GetLength(1); j++)
            {
                BuildTile(sectionGO.transform, section.PivotPosition, i, j, section.SectionTopography[i, j]);

                //if (section.SectionTopography[i,j] == GameData.TileType.Ground)
                //{
                //    Instantiate(Ground, pos, Quaternion.identity).name = section.SectionId + " Section. Ground Tile [" + i + ";" + j + "]";
                //}

                //if (section.SectionTopography[i, j] == GameData.TileType.Road)
                //{
                //    Instantiate(Road, pos, Quaternion.identity).name = section.SectionId + " Section.  Road Tile [" + i + ";" + j + "]";
                //}

                //if (section.SectionTopography[i, j] == GameData.TileType.Empty)
                //{
                //    Instantiate(Entrance, pos, Quaternion.identity).name = section.SectionId + " Section.  Empty Tile [" + i + ";" + j + "]";
                //}
            }
        }

        Debug.Log("Section Builded: " + section.SectionId);
    }

    GameObject BuildTile (Transform parent, Vector3 pivot, int x, int y, TileType type)
    {
        var pos = new Vector3();
        pos = pivot;
        pos.x += x;
        pos.z += y;

        switch (TileType)
        {
            case TileType.Empty:
            break;
            case TileType.Ground:
            break;
            case TileType.Road:
            break;
            case TileType.Bridge:
            break;
            default:
            break;
        }

        var Tile = Instantiate(Entrance, pos, Quaternion.identity);
        Tile.name = "Ground Tile [" + x + ";" + y + "]";
        Tile.transform.SetParent(parent);

        return Tile;
    }
}
