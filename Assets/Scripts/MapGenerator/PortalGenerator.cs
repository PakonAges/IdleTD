using GameData;
using System.Collections.Generic;
using UnityEngine;

public class PortalGenerator {

    NewMap myMap;
    SectionPositioner myPositioner;

    public List<Vector3> PortalList = new List<Vector3>();
    Dictionary<int,List<int>> SectionsLinias = new Dictionary<int, List<int>>();
    int iteration = 1;

	public PortalGenerator (NewMap map, SectionPositioner positioner)
    {
        myMap = map;
        myPositioner = positioner;
        GeneratePortals();
    }

    public void GeneratePortals()
    {
        BuildStartingSection();

        BuildNextLinia(SectionsLinias[iteration]);
    }

    void FindSectionTangency(int sectionId)
    {
        CheckSide(sectionId, Side.Right);
        CheckSide(sectionId, Side.Bot);
        CheckSide(sectionId, Side.Left);
        CheckSide(sectionId, Side.Top);
    }

    void CheckSide(int sectionId, Side side)
    {
        int sideLenght = 0;
        int cellToAdd = 0;
        bool isHorizontal = true;
        MapCell CheckCell = myPositioner.ConvertGlobalToLocal(myMap.MapSections[sectionId].PivotPosition);
        MapCell PortalCell = myPositioner.ConvertGlobalToLocal(myMap.MapSections[sectionId].PivotPosition);
        Dictionary<int, List<MapCell>> TangentCells = new Dictionary<int, List<MapCell>>();

        switch (side)
        {
            case Side.Top:
                sideLenght = myMap.MapSections[sectionId].Xsize;
                CheckCell.Y -= 2;
                PortalCell.Y -= 1;
                isHorizontal = true;
                break;
            case Side.Right:
                sideLenght = myMap.MapSections[sectionId].Ysize;
                CheckCell.X += myMap.MapSections[sectionId].Xsize + 1;
                PortalCell.X += myMap.MapSections[sectionId].Xsize;
                isHorizontal = false;
                break;
            case Side.Bot:
                sideLenght = myMap.MapSections[sectionId].Xsize;
                CheckCell.Y += myMap.MapSections[sectionId].Ysize + 1;
                PortalCell.Y += myMap.MapSections[sectionId].Ysize;
                isHorizontal = true;
                break;
            case Side.Left:
                sideLenght = myMap.MapSections[sectionId].Ysize;
                CheckCell.X -= 2;
                PortalCell.X -= 1;
                isHorizontal = false;
                break;
            default:
                break;
        }

        for (int i = 0; i < sideLenght; i++)
        {
            if (myPositioner.IsInMapLimits(CheckCell))
            {
                if (myPositioner.localMap[CheckCell.X,CheckCell.Y] != myPositioner.EmptyId && myPositioner.localMap[CheckCell.X, CheckCell.Y] != myPositioner.GapId)
                {
                    if (TangentCells.ContainsKey(myPositioner.localMap[CheckCell.X, CheckCell.Y]))
                    {
                        TangentCells[myPositioner.localMap[CheckCell.X, CheckCell.Y]].Add(PortalCell);
                    } else
                    {
                        TangentCells.Add(myPositioner.localMap[CheckCell.X, CheckCell.Y], new List<MapCell>());
                        TangentCells[myPositioner.localMap[CheckCell.X, CheckCell.Y]].Add(PortalCell);
                    }
                }
                
                if (isHorizontal)
                {
                    CheckCell.X++;
                    PortalCell.X++;
                } else
                {
                    CheckCell.Y++;
                    PortalCell.Y++;
                }
            }
        }

        foreach (KeyValuePair<int, List<MapCell>> pair in TangentCells)
        {
            if (!myMap.MapSections[pair.Key].DoesHaveAnExit)
            {
                cellToAdd = Random.Range(0, pair.Value.Count);
                PortalList.Add(myPositioner.ConvertLocalToGlobal(pair.Value[cellToAdd]));
                AddPortalExitToSections(sectionId, pair.Key, pair.Value[cellToAdd],side);

                myMap.MapSections[pair.Key].DoesHaveAnExit = true;

                if (SectionsLinias.ContainsKey(iteration))
                {
                    SectionsLinias[iteration].Add(pair.Key);
                } else
                {
                    SectionsLinias.Add(iteration, new List<int>());
                    SectionsLinias[iteration].Add(pair.Key);
                }
                
            }
        }
    }

    void AddPortalExitToSections(int FromSection, int ToSection, MapCell Portal, Side side)
    {
        int x = 0;
        int y = 0;

        switch (side)
        {
            case Side.Right:
                x = myMap.MapSections[FromSection].Xsize - 1;
                y = Portal.Y - myPositioner.ConvertGlobalToLocal(myMap.MapSections[FromSection].PivotPosition).Y;
                myMap.MapSections[FromSection].SectionTopography[x, y] = TileType.Road;
                myMap.MapSections[FromSection].ExitSides.Add(Side.Right);
                myMap.MapSections[FromSection].ExitCells.Add(new MapCell(x, y));

                x = 0;
                y = Portal.Y - myPositioner.ConvertGlobalToLocal(myMap.MapSections[ToSection].PivotPosition).Y;
                myMap.MapSections[ToSection].SectionTopography[x, y] = TileType.Road;
                myMap.MapSections[ToSection].Entrance = Side.Left;
                myMap.MapSections[ToSection].EntranceCell.X = x;
                myMap.MapSections[ToSection].EntranceCell.Y = y;
                break;
            case Side.Bot:
                x = Portal.X - myPositioner.ConvertGlobalToLocal(myMap.MapSections[FromSection].PivotPosition).X;
                y = myMap.MapSections[FromSection].Ysize - 1;
                myMap.MapSections[FromSection].SectionTopography[x, y] = TileType.Road;
                myMap.MapSections[FromSection].ExitSides.Add(Side.Bot);
                myMap.MapSections[FromSection].ExitCells.Add(new MapCell(x, y));

                x = Portal.X - myPositioner.ConvertGlobalToLocal(myMap.MapSections[ToSection].PivotPosition).X;
                y = 0;
                myMap.MapSections[ToSection].SectionTopography[x, y] = TileType.Road;
                myMap.MapSections[ToSection].Entrance = Side.Top;
                myMap.MapSections[ToSection].EntranceCell.X = x;
                myMap.MapSections[ToSection].EntranceCell.Y = y;
                break;
            case Side.Left:
                x = 0;
                y = Portal.Y - myPositioner.ConvertGlobalToLocal(myMap.MapSections[FromSection].PivotPosition).Y;
                myMap.MapSections[FromSection].SectionTopography[x, y] = TileType.Road;
                myMap.MapSections[FromSection].ExitSides.Add(Side.Left);
                myMap.MapSections[FromSection].ExitCells.Add(new MapCell(x, y));

                x = myMap.MapSections[ToSection].Xsize - 1;
                y = Portal.Y - myPositioner.ConvertGlobalToLocal(myMap.MapSections[ToSection].PivotPosition).Y;
                myMap.MapSections[ToSection].SectionTopography[x, y] = TileType.Road;
                myMap.MapSections[ToSection].Entrance = Side.Right;
                myMap.MapSections[ToSection].EntranceCell.X = x;
                myMap.MapSections[ToSection].EntranceCell.Y = y;
                break;
            case Side.Top:
                x = Portal.X - myPositioner.ConvertGlobalToLocal(myMap.MapSections[FromSection].PivotPosition).X;
                y = 0;
                myMap.MapSections[FromSection].SectionTopography[x, y] = TileType.Road;
                myMap.MapSections[FromSection].ExitSides.Add(Side.Top);
                myMap.MapSections[FromSection].ExitCells.Add(new MapCell(x, y));

                x = Portal.X - myPositioner.ConvertGlobalToLocal(myMap.MapSections[ToSection].PivotPosition).X;
                y = myMap.MapSections[ToSection].Ysize - 1;
                myMap.MapSections[ToSection].SectionTopography[x, y] = TileType.Road;
                myMap.MapSections[ToSection].Entrance = Side.Bot;
                myMap.MapSections[ToSection].EntranceCell.X = x;
                myMap.MapSections[ToSection].EntranceCell.Y = y;
                break;
            default:
                break;
        }
    }

    void BuildStartingSection()
    {
        FindSectionTangency(1);
        myMap.MapSections[1].DoesHaveAnExit = true;
        myMap.MapSections[1].Entrance = Side.Top;
    }

    void BuildNextLinia(List<int> queue)
    {
        iteration++;

        foreach (int SectionId in queue)
        {
            FindSectionTangency(SectionId);
        }

        if (SectionsLinias.ContainsKey(iteration))
        {
            BuildNextLinia(SectionsLinias[iteration]);
        }
    }
}
