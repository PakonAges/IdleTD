using GameData;
using System.Collections.Generic;
using UnityEngine;

public class CreepWayBuilder {

    public Dictionary<int, List<Vector2>> pathInSections = new Dictionary<int, List<Vector2>>();

    public CreepWayBuilder(NewMap map)
    {
        foreach (var section in map.MapSections)
        {
            pathInSections.Add(section.Key, GeneratePathInSection(section.Value));
        }
    }

    List<Vector2> GeneratePathInSection(MapSection section)
    {
        List<Vector2> localPath = new List<Vector2>();
        List<Vector2> RoadList = new List<Vector2>();

        Vector2 focusCell = new Vector2(section.EntranceCell.X, section.EntranceCell.Y);
        Vector2 checkCell = new Vector2();     //cell on the clockwise side of the movement to find road turn

        Vector2 prevCell = new Vector2(focusCell.x,focusCell.y);        //To add wayPoint on the previous cell, if road is changing direction
        Vector2 startCell = new Vector2(focusCell.x, focusCell.y);      //To check when to stop. After a full circle

        Side direction = StartDirection(section.Entrance);

        for (int i = 0; i < section.SectionTopography.GetLength(0); i++)
        {
            for (int y = 0; y < section.SectionTopography.GetLength(1); y++)
            {
                if (section.SectionTopography[i, y] == TileType.Road)
                {
                    RoadList.Add(new Vector2(i, y));
                }
            }
        }

        // AddWayPointToList(focusCell, section.PivotPosition);
        localPath.Add(CalibrateCell(focusCell, section.PivotPosition));

        for (int i = 0; i < RoadList.Count*2; i++)
        {
            switch (direction) //Not a Side! but a direction!
            {
                case Side.None:
                    break;
                case Side.Top:
                    checkCell.x = focusCell.x + 1;
                    checkCell.y = focusCell.y;
                    focusCell.y--;
                    break;
                case Side.Right:
                    checkCell.x = focusCell.x;
                    checkCell.y = focusCell.y + 1;
                    focusCell.x++;
                    break;
                case Side.Bot:
                    checkCell.x = focusCell.x - 1;
                    checkCell.y = focusCell.y;
                    focusCell.y++;
                    break;
                case Side.Left:
                    checkCell.x = focusCell.x;
                    checkCell.y = focusCell.y - 1;
                    focusCell.x--;
                    break;
                default:
                    break;
            }

            if (focusCell == startCell)
            {
                break;
            }

            if (!RoadList.Contains(focusCell))
             {
                if (prevCell != startCell && !localPath.Contains(prevCell))
                {
                    //AddWayPointToList(prevCell, section.PivotPosition);
                    localPath.Add(CalibrateCell(prevCell, section.PivotPosition));
                }

                focusCell.x = prevCell.x;
                focusCell.y = prevCell.y;

                direction = ChangeDirection(direction, RoadList.Contains(checkCell));
            }
            else
            {
                prevCell.x = focusCell.x;
                prevCell.y = focusCell.y;

                //Add exit point as a wayPoint if it exist
                if (section.ExitCells.Contains(new MapCell((int)focusCell.x, (int)focusCell.y)) && !localPath.Contains(focusCell))
                {
                    //AddWayPointToList(focusCell, section.PivotPosition);
                    localPath.Add(CalibrateCell(focusCell, section.PivotPosition));
                }
            }
        }

        return localPath;
    }

    Side StartDirection(Side direction)
    {
        switch (direction)
        {
            case Side.None:
                return Side.None;
                
            case Side.Top:
                return Side.Right;
                
            case Side.Right:
                return Side.Bot;

            case Side.Bot:
                return Side.Left;

            case Side.Left:
                return Side.Top;

            default:
                return Side.None;
        }
    }

    Side ChangeDirection(Side direction, bool CheckCell)
    {
        switch (direction)
        {
            case Side.None:
                return Side.None;

            case Side.Top:
                if (CheckCell)
                {
                    return Side.Right;
                }
                return Side.Left;

            case Side.Right:
                if (CheckCell)
                {
                    return Side.Bot;
                }
                return Side.Top;

            case Side.Bot:
                if (CheckCell)
                {
                    return Side.Left;
                }
                return Side.Right;

            case Side.Left:
                if (CheckCell)
                {
                    return Side.Top;
                }
                return Side.Bot;

            default:
                return Side.None;
        }
    }

    Vector2 CalibrateCell(Vector2 wp, Vector3 pivot)
    {
        wp.x += pivot.x;
        wp.y -= pivot.z;

        return wp;
    }
}
