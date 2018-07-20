using UnityEngine;
using GameData;
using System.Collections.Generic;

public class MapSection {

    public int SectionId;

    public Vector3 PivotPosition;
    public int Xsize;
    public int Ysize;
    public TileType[,] SectionTopography;

    public bool DoesHaveAnExit = false;

    public Side EntranceSide = Side.None;
    public MapCell EntranceCell = new MapCell();

    public List<Side> ExitSides = new List<Side>();
    public List<MapCell> ExitCells = new List<MapCell>();

    public bool IsUnlocked = false;

    public MapSection(int id, GlobalSectionCell section)
    {
        SectionId = id;
        PivotPosition = section.Position;
        Xsize = section.Xsize;
        Ysize = section.Ysize;
        SectionTopography = new TileType[Xsize, Ysize];
    }
}