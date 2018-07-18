using GameData;
using System;
using System.Collections.Generic;
using UnityEngine;

public class SectionRoadBuilder
{

    //int roadLenght = 0;
    //int startingPoint = 0;

    int totalRounds = 0;
    int round = 0;

    public SectionRoadBuilder(Dictionary<int, MapSection> map)
    {

        foreach (KeyValuePair<int, MapSection> mapSection in map)
        {
            if (mapSection.Key == 1)
            {
                mapSection.Value.SectionTopography[mapSection.Value.Xsize / 2, 0] = TileType.Road;
                mapSection.Value.EntranceSide = Side.Top;
                mapSection.Value.EntranceCell = new MapCell(mapSection.Value.Xsize / 2, 0);
            }

            TileType[,] BuildedSection = GenerateRoad(mapSection.Value.SectionTopography);

            while (!IsSectionValid(BuildedSection))
            {
                BuildedSection = GenerateRoad(mapSection.Value.SectionTopography);
            }

            mapSection.Value.SectionTopography = BuildedSection;
        }
    }

    TileType[,] GenerateRoad(TileType[,] Section)
    {
        TileType[,] BuildedSection = Section.Clone() as TileType[,];


        //TileType[,] BuildedSection = section.SectionTopography;

        //section.SectionTopography[1, 0] = TileType.Road;
        //section.SectionTopography[5, 0] = TileType.Road;
        //totalRounds = CalcRounds(section.Xsize, section.Ysize);

        totalRounds = 1;
        round = 0;

        for (int i = 0; i < totalRounds; i++)
        {
            if (round == 0)
            {
                CheckSidesFor3LongRoads(BuildedSection);
                BuildPerimeterRoad(BuildedSection, Side.Top);
                CleanUpRoads(BuildedSection);
                BuildPerimeterRoad(BuildedSection, Side.Right);
                CleanUpRoads(BuildedSection);
                BuildPerimeterRoad(BuildedSection, Side.Bot);
                CleanUpRoads(BuildedSection);
                BuildPerimeterRoad(BuildedSection, Side.Left);

                CleanUpRoads(BuildedSection);
            }

            round++;

            BuildInnerRoads(BuildedSection);
            FillCenter(BuildedSection);
        }

        return BuildedSection;

    }



    int CalcRounds(int x, int y)
    {
        if (x > y)
        {
            return (int)Math.Ceiling(y * 0.5f);
        }
        else
        {
            return (int)Math.Ceiling(x * 0.5f);
        }
    }



    void BuildPerimeterRoad(TileType[,] section, Side side)
    {
        int? firstEmptyPoint = FindFirstCellOfType(section, side, TileType.Empty);
        int? secondEmptyPoint = FindLastCellOfType(section, side, TileType.Empty);
        int? firstRoadPoint = FindFirstCellOfType(section, side, TileType.Road);
        int? secondRoadPoint = FindLastCellOfType(section, side, TileType.Road);

        int min;
        int max;

        int minRoad = CalcMinRoad(section, side);

        //No empty cells
        if (firstEmptyPoint == null || secondEmptyPoint == null)
        {
            return;
        }

        //First road  exist and it is earlier that first empty point
        if (firstRoadPoint != null && firstRoadPoint < firstEmptyPoint)
        {
            min = (int)firstRoadPoint;
        }
        else
        {
            min = (int)firstEmptyPoint;
        }

        //second road always should exist and if it's on the right side:
        if (secondRoadPoint > secondEmptyPoint)
        {
            max = (int)secondRoadPoint;
        }
        else
        {
            max = (int)secondEmptyPoint;
        }

        int maxLenght = max - min + 1;

        int startPoint = 0;
        int roadLenght = 0;

        //no roads
        if (firstRoadPoint == null && secondRoadPoint == null)
        {
            roadLenght = Rand(minRoad, maxLenght);
            startPoint = Rand((int)firstEmptyPoint, (int)secondEmptyPoint - roadLenght + 1);
        }

        //one road cell
        if (firstRoadPoint != null && firstRoadPoint == secondRoadPoint)
        {
            roadLenght = Rand(minRoad, maxLenght);

            if (firstRoadPoint == min)
            {
                startPoint = min;
            }
            else
            {
                if (secondRoadPoint == max)
                {
                    startPoint = max - roadLenght + 1;
                }
                else
                {
                    startPoint = Rand(min, max, (int)firstRoadPoint, roadLenght);
                }
            }
        }

        //two or more road pieces
        if (firstRoadPoint != null && secondRoadPoint != firstRoadPoint)
        {
            roadLenght = Rand((int)(secondRoadPoint - firstRoadPoint + 1), maxLenght);

            if (roadLenght < minRoad)
            {
                roadLenght = minRoad;
            }

            startPoint = Rand(min, max, (int)firstRoadPoint, (int)secondRoadPoint, roadLenght);
        }

        if (roadLenght > maxLenght)
        {
            Debug.Log("AHTUNG! Trying to build road: " + roadLenght + ". In Section: " + section.GetLength(0) + "x" + section.GetLength(1));
        }

        if (roadLenght >= minRoad)
        {
            PlaceRoad(section, side, startPoint, roadLenght);
        }

        CheckSidesFor3LongRoads(section);
    }



    void BuildInnerRoads(TileType[,] section)
    {
        for (int i = 1; i < section.GetLength(0) - 1; i++)
        {
            if (section[i, 1] == TileType.Empty)
            {
                section[i, 1] = TileType.Road;
            }
            if (section[i, section.GetLength(1) - 2] == TileType.Empty)
            {
                section[i, section.GetLength(1) - 2] = TileType.Road;
            }
        }

        for (int y = 1; y < section.GetLength(1) - 1; y++)
        {
            if (section[1, y] == TileType.Empty)
            {
                section[1, y] = TileType.Road;
            }
            if (section[section.GetLength(0) - 2, y] == TileType.Empty)
            {
                section[section.GetLength(0) - 2, y] = TileType.Road;
            }
        }

    }



    private void FillCenter(TileType[,] section)
    {
        for (int i = 1; i < section.GetLength(0) - 1; i++)
        {
            for (int k = 1; k < section.GetLength(1) - 1; k++)
            {
                if (section[i, k] == TileType.Empty)
                {
                    section[i, k] = TileType.Ground;
                }
            }
        }
    }



    void CheckSidesFor3LongRoads(TileType[,] section)
    {
        if (PotentialRoadSize(section, Side.Top) == 3)
        {
            PlaceRoad(section, Side.Top, (int)FindFirstCellOfType(section, Side.Top, TileType.Empty, TileType.Road), 3);
        }

        if (PotentialRoadSize(section, Side.Right) == 3)
        {
            PlaceRoad(section, Side.Right, (int)FindFirstCellOfType(section, Side.Right, TileType.Empty, TileType.Road), 3);
        }

        if (PotentialRoadSize(section, Side.Bot) == 3)
        {
            PlaceRoad(section, Side.Bot, (int)FindFirstCellOfType(section, Side.Bot, TileType.Empty, TileType.Road), 3);
        }

        if (PotentialRoadSize(section, Side.Left) == 3)
        {
            PlaceRoad(section, Side.Left, (int)FindFirstCellOfType(section, Side.Left, TileType.Empty, TileType.Road), 3);
        }
    }



    void PlaceRoad(TileType[,] section, Side side, int Start, int Lenght)
    {
        int a = 0;
        int b = 0;

        for (int i = 0; i < Lenght; i++)
        {
            switch (side)
            {
                case Side.None:
                    break;
                case Side.Top:
                    a = Start + i;
                    b = 0 + round;
                    break;
                case Side.Right:
                    a = section.GetLength(0) - 1 - round;
                    b = Start + i;
                    break;
                case Side.Bot:
                    a = Start + i;
                    b = section.GetLength(1) - 1 - round;
                    break;
                case Side.Left:
                    a = 0 + round;
                    b = Start + i;
                    break;
                default:
                    break;
            }

            section[a, b] = TileType.Road;
        }

        PlaceGround(section, side);
        PlaceGroundBetweenExits(section, side, Start, Lenght);
        PlaceRoadEndsInward(section, side, Start, Lenght);
    }



    void PlaceGround(TileType[,] section, Side side)
    {
        int a = 0;
        int b = 0;
        int l = 0;

        if (side == Side.Top || side == Side.Bot)
        {
            l = section.GetLength(0) - 2 * round;
        }
        else
        {
            l = section.GetLength(1) - 2 * round;
        }


        for (int i = round; i < l; i++)
        {
            switch (side)
            {
                case Side.None:
                    break;
                case Side.Top:
                    a = i;
                    b = round;
                    break;
                case Side.Right:
                    a = section.GetLength(0) - round - 1;
                    b = i;
                    break;
                case Side.Bot:
                    a = i;
                    b = section.GetLength(1) - round - 1;
                    break;
                case Side.Left:
                    a = round;
                    b = i;
                    break;
                default:
                    break;
            }

            if (section[a, b] == TileType.Empty)
            {
                section[a, b] = TileType.Ground;
            }
        }
    }



    void PlaceGroundBetweenExits(TileType[,] section, Side side, int RoadStart, int RoadLenght)
    {
        int a = 0;
        int b = 0;

        for (int i = 1; i < RoadLenght - 1; i++)
        {
            switch (side)
            {
                case Side.None:
                    break;
                case Side.Top:
                    a = RoadStart + i;
                    b = 1 + round;
                    break;
                case Side.Right:
                    a = section.GetLength(0) - 2 - round;
                    b = RoadStart + i;
                    break;
                case Side.Bot:
                    a = RoadStart + i;
                    b = section.GetLength(1) - 2 - round;
                    break;
                case Side.Left:
                    a = 1 + round;
                    b = RoadStart + i;
                    break;
                default:
                    break;
            }

            section[a, b] = TileType.Ground;
        }
    }



    void PlaceRoadEndsInward(TileType[,] section, Side side, int RoadStart, int RoadLenght)
    {
        int a = 0;
        int b = 0;

        for (int i = 0; i < RoadLenght; i++)
        {
            if (i == 0 || i == RoadLenght - 1)
            {
                switch (side)
                {
                    case Side.None:
                        break;
                    case Side.Top:
                        a = RoadStart + i;
                        b = 1 + round;
                        break;
                    case Side.Right:
                        a = section.GetLength(0) - 2 - round;
                        b = RoadStart + i;
                        break;
                    case Side.Bot:
                        a = RoadStart + i;
                        b = section.GetLength(1) - 2 - round;
                        break;
                    case Side.Left:
                        a = 1 + round;
                        b = RoadStart + i;
                        break;
                    default:
                        break;
                }

                section[a, b] = TileType.Road;
            }
        }
    }


    //3 for short side, 4 for long side
    private int CalcMinRoad(TileType[,] section, Side side)
    {
        if (section.GetLength(0) == section.GetLength(1))
        {
            return 3;
        }

        if (section.GetLength(0) > section.GetLength(1))
        {
            if (side == Side.Top || side == Side.Bot)
            {
                return 4;
            }
            else
            {
                return 3;
            }
        }
        else
        {
            if (side == Side.Top || side == Side.Bot)
            {
                return 3;
            }
            else
            {
                return 4;
            }
        }
    }



    private void CleanUpRoads(TileType[,] section)
    {
        //exclude utmost cells
        for (int i = 1; i < section.GetLength(0) - 1; i++)
        {
            for (int k = 1; k < section.GetLength(1) - 1; k++)
            {
                if (section[i, k] == TileType.Road)
                {
                    CheckCellForRoads(section, i, k);
                }
            }
        }
    }



    private void CheckCellForRoads(TileType[,] section, int x, int y)
    {
        int RoadsAround = 0;
        int EmptyCellsAround = 0;
        Dictionary<MapCell, TileType> CellsAround = new Dictionary<MapCell, TileType>();

        CellsAround.Add(new MapCell(x, y - 1), section[x, y - 1]);
        CellsAround.Add(new MapCell(x + 1, y), section[x + 1, y]);
        CellsAround.Add(new MapCell(x, y + 1), section[x, y + 1]);
        CellsAround.Add(new MapCell(x - 1, y), section[x - 1, y]);

        foreach (var pair in CellsAround)
        {
            if (pair.Value == TileType.Road)
            {
                RoadsAround++;
            }

            if (pair.Value == TileType.Empty)
            {
                EmptyCellsAround++;
            }

        }

        if (RoadsAround == 1 && EmptyCellsAround == 1)
        {
            foreach (var pair in CellsAround)
            {
                if (section[pair.Key.X, pair.Key.Y] == TileType.Empty)
                {
                    section[pair.Key.X, pair.Key.Y] = TileType.Road;
                    CleanUpRoads(section);
                    return;
                }
            }
        }

        if (RoadsAround == 2)
        {
            foreach (var pair in CellsAround)
            {
                if (section[pair.Key.X, pair.Key.Y] == TileType.Empty)
                {
                    section[pair.Key.X, pair.Key.Y] = TileType.Ground;
                }
            }
            return;
        }
    }


    //Helpers Methods ===================== /
    int PotentialRoadSize(TileType[,] section, Side side)
    {
        int sideLenght;
        int potentialRoadLenght = 0;

        if (side == Side.Top || side == Side.Bot)
        {
            sideLenght = section.GetLength(0) - 2 * round;
        }
        else
        {
            sideLenght = section.GetLength(1) - 2 * round;
        }

        for (int i = 0; i < sideLenght; i++)
        {
            int a = 0;
            int b = 0;

            switch (side)
            {
                case Side.None:
                    Debug.Log("AHTUNG! Something is wrong");
                    break;
                case Side.Top:
                    a = i;
                    b = round;
                    break;
                case Side.Right:
                    a = section.GetLength(0) - 1 - round;
                    b = i;
                    break;
                case Side.Bot:
                    a = i;
                    b = section.GetLength(1) - 1 - round;
                    break;
                case Side.Left:
                    a = round;
                    b = i;
                    break;
                default:
                    break;
            }

            if (section[a, b] == TileType.Empty || section[a, b] == TileType.Road)
            {
                potentialRoadLenght++;
            }
        }

        return potentialRoadLenght;
    }


    //Find Cell of Type
    private int? FindFirstCellOfType(TileType[,] section, Side side, TileType typeNeeded)
    {
        int sideLenght;
        int? firstEmptyCell = null;
        int a = 0;
        int b = 0;

        if (side == Side.Top || side == Side.Bot)
        {
            sideLenght = section.GetLength(0) - 2 * round;
        }
        else
        {
            sideLenght = section.GetLength(1) - 2 * round;
        }

        for (int i = 0; i < sideLenght; i++)
        {
            switch (side)
            {
                case Side.None:
                    Debug.Log("AHTUNG! Something is wrong #2");
                    break;
                case Side.Top:
                    a = round + i;
                    b = round;
                    break;
                case Side.Right:
                    a = section.GetLength(0) - 1 - round;
                    b = round + i;
                    break;
                case Side.Bot:
                    a = round + i;
                    b = section.GetLength(1) - 1 - round;
                    break;
                case Side.Left:
                    a = round;
                    b = round + i;
                    break;
                default:
                    break;
            }

            if (section[a, b] == typeNeeded)
            {
                if (side == Side.Top || side == Side.Bot)
                {
                    firstEmptyCell = a;
                }
                else
                {
                    firstEmptyCell = b;
                }

                break;
            }
        }

        return firstEmptyCell;
    }


    //Find Cell of Type1 or Type2
    private int? FindFirstCellOfType(TileType[,] section, Side side, TileType firstType, TileType secondType)
    {
        int sideLenght;
        int? firstEmptyCell = null;
        int a = 0;
        int b = 0;

        if (side == Side.Top || side == Side.Bot)
        {
            sideLenght = section.GetLength(0) - 2 * round;
        }
        else
        {
            sideLenght = section.GetLength(1) - 2 * round;
        }

        for (int i = 0; i < sideLenght; i++)
        {
            switch (side)
            {
                case Side.None:
                    Debug.Log("AHTUNG! Something is wrong #5");
                    break;
                case Side.Top:
                    a = round + i;
                    b = round;
                    break;
                case Side.Right:
                    a = section.GetLength(0) - 1 - round;
                    b = round + i;
                    break;
                case Side.Bot:
                    a = round + i;
                    b = section.GetLength(1) - 1 - round;
                    break;
                case Side.Left:
                    a = round;
                    b = round + i;
                    break;
                default:
                    break;
            }

            if (section[a, b] == firstType || section[a, b] == secondType)
            {
                if (side == Side.Top || side == Side.Bot)
                {
                    firstEmptyCell = a;
                }
                else
                {
                    firstEmptyCell = b;
                }

                break;
            }
        }

        return firstEmptyCell;
    }



    private int? FindLastCellOfType(TileType[,] section, Side side, TileType typeNeeded)
    {
        int sideLenght;
        int? lastEmptyCell = null;
        int a = 0;
        int b = 0;

        if (side == Side.Top || side == Side.Bot)
        {
            sideLenght = section.GetLength(0) - 2 * round;
        }
        else
        {
            sideLenght = section.GetLength(1) - 2 * round;
        }

        for (int i = 0; i < sideLenght; i++)
        {
            switch (side)
            {
                case Side.None:
                    Debug.Log("AHTUNG! Something is wrong #3");
                    break;
                case Side.Top:
                    a = section.GetLength(0) - 1 - round - i;
                    b = round;
                    break;
                case Side.Right:
                    a = section.GetLength(0) - 1 - round;
                    b = section.GetLength(1) - 1 - round - i;
                    break;
                case Side.Bot:
                    a = section.GetLength(0) - 1 - round - i;
                    b = section.GetLength(1) - 1 - round;
                    break;
                case Side.Left:
                    a = round;
                    b = section.GetLength(1) - 1 - round - i;
                    break;
                default:
                    break;
            }

            if (section[a, b] == typeNeeded)
            {
                if (side == Side.Top || side == Side.Bot)
                {
                    lastEmptyCell = a;
                }
                else
                {
                    lastEmptyCell = b;
                }

                break;
            }
        }

        return lastEmptyCell;
    }



    int Rand(int min, int max)
    {
        return UnityEngine.Random.Range(min, max + 1);
    }


    //japaneese crossword principe with 1 dot
    int Rand(int min, int max, int dotIndex, int RoadLenght)
    {
        int a;
        int b;

        if (dotIndex - RoadLenght < min)
        {
            a = min;

            if (max - RoadLenght < dotIndex)
            {
                b = max - RoadLenght + 1;
            }
            else
            {
                b = dotIndex;
            }
        }
        else
        {
            a = dotIndex - RoadLenght + 1;
            b = max - RoadLenght + 1;
        }

        return Rand(a, b);
    }


    //japaneese crossword principe with 2 dots
    int Rand(int min, int max, int dot1, int dot2, int RoadLenght)
    {
        int a;
        int b;

        if (dot2 - RoadLenght < min)
        {
            a = min;
        }
        else
        {
            a = dot2 - RoadLenght + 1;
        }

        if (dot1 + RoadLenght > max)
        {
            b = max - RoadLenght + 1;
        }
        else
        {
            b = dot1;
        }

        return Rand(a, b);
    }



    bool IsSectionValid(TileType[,] thisSection)
    {
        for (int i = 0; i < thisSection.GetLength(0); i++)
        {
            for (int k = 0; k < thisSection.GetLength(1); k++)
            {
                if (thisSection[i, k] == TileType.Road && !IsRoadValid(thisSection, i, k))
                {
                    return false;
                }
            }
        }

        return true;
    }

    bool IsRoadValid(TileType[,] thisSection, int i, int k)
    {
        int roadsAround = 0;

        //left
        if (i != 0)
        {
            if (thisSection[i - 1, k] == TileType.Road)
            {
                roadsAround++;
            }
        }

        //top
        if (k != thisSection.GetLength(1) - 1)
        {
            if (thisSection[i, k + 1] == TileType.Road)
            {
                roadsAround++;
            }
        }

        //right
        if (i != thisSection.GetLength(0) - 1)
        {
            if (thisSection[i + 1, k] == TileType.Road)
            {
                roadsAround++;
            }
        }

        //bot
        if (k != 0)
        {
            if (thisSection[i, k - 1] == TileType.Road)
            {
                roadsAround++;
            }
        }

        if (roadsAround != 2)
        {
            return false;
        }
        else return true;
    }
}