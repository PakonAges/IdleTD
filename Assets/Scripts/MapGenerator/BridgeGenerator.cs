using GameData;
using System.Collections.Generic;
using UnityEngine;

public class BridgeGenerator {

    Map myMap;
    SectionPositioner myPositioner;

    public List<Bridge> Bridges = new List<Bridge>();
    Dictionary<int,List<int>> SectionLanes = new Dictionary<int, List<int>>();
    int iteration = 1;

	public BridgeGenerator (Map map, SectionPositioner positioner)
    {
        myMap = map;
        myPositioner = positioner;
        GenerateBridges();

        for (int i = 0; i < Bridges.Count; i++)
        {
            map.Bridges.Add(Bridges[i]);
        }
    }

    public void GenerateBridges()
    {
        BuildStartingSection();
        BuildNextLane(SectionLanes[iteration]);
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
        MapCell BridgeStartCell = myPositioner.ConvertGlobalToLocal(myMap.MapSections[sectionId].PivotPosition);
        Dictionary<int, List<MapCell>> TangentCells = new Dictionary<int, List<MapCell>>();

        switch (side)
        {
            case Side.Top:
                sideLenght = myMap.MapSections[sectionId].Xsize;
                CheckCell.Y -= (myPositioner.Gap + 1);
                BridgeStartCell.Y -= 1;
                isHorizontal = true;
                break;
            case Side.Right:
                sideLenght = myMap.MapSections[sectionId].Ysize;
                CheckCell.X += myMap.MapSections[sectionId].Xsize + myPositioner.Gap;
                BridgeStartCell.X += myMap.MapSections[sectionId].Xsize;
                isHorizontal = false;
                break;
            case Side.Bot:
                sideLenght = myMap.MapSections[sectionId].Xsize;
                CheckCell.Y += myMap.MapSections[sectionId].Ysize + myPositioner.Gap;
                BridgeStartCell.Y += myMap.MapSections[sectionId].Ysize;
                isHorizontal = true;
                break;
            case Side.Left:
                sideLenght = myMap.MapSections[sectionId].Ysize;
                CheckCell.X -= (myPositioner.Gap + 1);
                BridgeStartCell.X -= 1;
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
                        TangentCells[myPositioner.localMap[CheckCell.X, CheckCell.Y]].Add(BridgeStartCell);
                    } else
                    {
                        TangentCells.Add(myPositioner.localMap[CheckCell.X, CheckCell.Y], new List<MapCell>());
                        TangentCells[myPositioner.localMap[CheckCell.X, CheckCell.Y]].Add(BridgeStartCell);
                    }
                }
                
                if (isHorizontal)
                {
                    CheckCell.X++;
                    BridgeStartCell.X++;
                } else
                {
                    CheckCell.Y++;
                    BridgeStartCell.Y++;
                }
            }
        }

        foreach (KeyValuePair<int, List<MapCell>> pair in TangentCells)
        {
            if (!myMap.MapSections[pair.Key].DoesHaveAnExit)
            {
                cellToAdd = Random.Range(0, pair.Value.Count);

                Bridges.Add(BuildBridge(pair.Value[cellToAdd], side));

                AddPortalExitToSections(sectionId, pair.Key, pair.Value[cellToAdd],side);

                myMap.MapSections[pair.Key].DoesHaveAnExit = true;

                if (SectionLanes.ContainsKey(iteration))
                {
                    SectionLanes[iteration].Add(pair.Key);
                } else
                {
                    SectionLanes.Add(iteration, new List<int>());
                    SectionLanes[iteration].Add(pair.Key);
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
                myMap.MapSections[ToSection].EntranceSide = Side.Left;
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
                myMap.MapSections[ToSection].EntranceSide = Side.Top;
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
                myMap.MapSections[ToSection].EntranceSide = Side.Right;
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
                myMap.MapSections[ToSection].EntranceSide = Side.Bot;
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
        if (myMap.MapSections.Count > 1)
        {
            myMap.MapSections[1].DoesHaveAnExit = true;
        }
        else
        {
            myMap.MapSections[1].DoesHaveAnExit = false;
        }

        myMap.MapSections[1].EntranceSide = Side.Top;
    }

    void BuildNextLane(List<int> queue)
    {
        iteration++;

        foreach (int SectionId in queue)
        {
            FindSectionTangency(SectionId);
        }

        if (SectionLanes.ContainsKey(iteration))
        {
            BuildNextLane(SectionLanes[iteration]);
        }
    }

    private Bridge BuildBridge(MapCell bridgeStart, Side side)
    {
        var newBridge = new Bridge(myPositioner.Gap);

        for (int i = 0; i < newBridge.BridgeTiles.Length; i++)
        {
            var BridgeTile = myPositioner.ConvertLocalToGlobal(bridgeStart);
            switch (side)
            {
                case Side.None:
                break;

                case Side.Top:
                    BridgeTile.z += i;
                break;

                case Side.Right:
                    BridgeTile.x += i;
                break;

                case Side.Bot:
                BridgeTile.z -= i;
                break;

                case Side.Left:
                BridgeTile.x -= i;
                break;

                default:
                break;
            }

            newBridge.BridgeTiles[i] = BridgeTile;
        }

        return newBridge;
    }
}
