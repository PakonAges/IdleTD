﻿using UnityEngine;
using GameData;
using System.Collections.Generic;

public class MapBuilder : MonoBehaviour {

    public GameObject RoadTile;
    public GameObject GroundTile;
    public GameObject Bridge;

    public void BuildMap(Dictionary<int, MapSection> map, List<Vector3> portals)
    {
        BuildEntrance(map[1].Xsize / 2);

        foreach (KeyValuePair<int, MapSection> pair in map)
        {
            BuildMapSection(pair.Value);
        }

        foreach (var portal in portals)
        {
            Instantiate(RoadTile, portal, Quaternion.identity,transform).name = "Portal";
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

        GameObject sectionGO = new GameObject();
        sectionGO.name = "MapSection (" + SectionX + ";" + SectionY + ")";
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

    }

}