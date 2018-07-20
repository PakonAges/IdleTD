using System;
using System.Collections;
using System.Collections.Generic;
using GameData;
using UnityEngine;

public class SectionObjectsSpawner : MonoBehaviour {

    public List<GameObject> RocksSize1 = new List<GameObject>();

    public void SpawnRocks(Dictionary<int, MapSection> map)
    {
        foreach (KeyValuePair<int, MapSection> section in map)
        {
            SpawnRocksInSection(section.Value.SectionTopography, section.Value.PivotPosition);
        }
    }


    void SpawnRocksInSection(TileType[,] section, Vector3 pivot)
    {
        int SectionX = section.GetLength(0);
        int SectionY = section.GetLength(1);

        for (int i = 0; i < SectionX; i++)
        {
            for (int y = 0; y < SectionY; y++)
            {
                if (section[i,y] == TileType.Ground)
                {
                    CreateRock(i, y, pivot);
                }
            }
        }

    }


    void CreateRock(int i, int y, Vector3 pivot)
    {
        Vector3 position = new Vector3(i + pivot.x, pivot.y, -y + pivot.z);
        GameObject rockToBuild = RocksSize1[UnityEngine.Random.Range(0,RocksSize1.Count)];
        RotateRandomnly(Instantiate(rockToBuild, position, Quaternion.identity));
    }


    void RotateRandomnly(GameObject obj)
    {
        int rotation = UnityEngine.Random.Range(0, 4);
        int angle = 0;

        switch (rotation)
        {
            case 0:
                angle = 0;
                break;

            case 1:
                angle = 90;
                break;

            case 2:
                angle = 180;
                break;

            case 3:
                angle = 270;
                break;

            default:
                break;
        }

        obj.transform.Rotate(Vector3.up, angle);
    }
}
