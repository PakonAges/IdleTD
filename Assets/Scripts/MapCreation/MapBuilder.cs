using UnityEngine;
using GameData;
using System.Collections.Generic;
using Zenject;

/// <summary>
/// Builds level by the Data from the Generator. Including all objects, light, etc. ?s
/// </summary>
public class MapBuilder : MonoBehaviour {

    private MapGenerator _mapGenerator;

    public GameObject RoadTile;
    public GameObject GroundTile;
    public GameObject Bridge;
    public GameObject SectionPillar;

    [Inject]
    public void Construct(MapGenerator mapGenerator)
    {
        _mapGenerator = mapGenerator;
    }

    public void BuildMap()
    {
        var map = _mapGenerator.GenerateMap();

        BuildEntrance(map.MapSections[1].Xsize / 2);

        foreach (KeyValuePair<int, MapSection> pair in map.MapSections)
        {
            BuildMapSection(pair.Value);
        }

        foreach (var bridge in map.Bridges)
        {
            for (int i = 0; i < bridge.BridgeTiles.Length; i++)
            {
                Instantiate(RoadTile, bridge.BridgeTiles[i], Quaternion.identity, transform).name = "Bridge";
            }
        }
    }

    void BuildEntrance(int place)//HACK
    {
        Instantiate(RoadTile, new Vector3(place, 0, 2), Quaternion.identity,gameObject.transform).name = "EntranceRoad 1";
        Instantiate(RoadTile, new Vector3(place, 0, 1), Quaternion.identity,gameObject.transform).name = "EntranceRoad 2";
    }

    public void BuildMapSection(MapSection section)
    {
        int SectionX = section.SectionTopography.GetLength(0);
        int SectionY = section.SectionTopography.GetLength(1);

        GameObject sectionGO = new GameObject
        {
            name = "MapSection (" + SectionX + ";" + SectionY + ")"
        };

        sectionGO.transform.SetParent(this.transform);
        
        for (int i = 0; i < SectionX; i++)
        {
            for (int y = 0; y < SectionY; y++)
            {
                Vector3 position = new Vector3(i, 0, -y);
                TileType tileType = section.SectionTopography[i,y];
                GameObject tileToBuild = null;

                switch (tileType)
                {
                    case TileType.Empty:
                        break;
                    case TileType.Ground:
                        tileToBuild = GroundTile;
                        break;
                    case TileType.Road:
                        tileToBuild = RoadTile;
                        break;
                    case TileType.Bridge:
                        tileToBuild = Bridge;
                        break;
                    default:
                        break;
                }

                if (tileToBuild != null)
                {
                    Instantiate(tileToBuild, position, Quaternion.identity, sectionGO.transform);
                }
                    
                else
                {
                    Debug.Log("won't instantiate null prefab at: (" + i + ";" + y + ")" );
                }
            }
        }

        sectionGO.transform.position = section.PivotPosition;

        BuildPillar(section);

    }

    public void BuildPillar(MapSection section)
    {
        var PivotOffset = new Vector3(section.Xsize * 0.5f - 0.5f, -1, -section.Ysize * 0.5f + 0.5f);

        var pillar = Instantiate(SectionPillar);
        pillar.name = "Section Pillar " + section.SectionId;
        pillar.transform.localScale = new Vector3(section.Xsize, 10, section.Ysize);
        pillar.transform.position = section.PivotPosition + PivotOffset;
    }
}