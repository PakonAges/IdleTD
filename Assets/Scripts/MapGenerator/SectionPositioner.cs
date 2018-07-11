using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SectionPositioner {

    int Gap;
    public int GapId = 666;

    public int EmptyId = 0;

    int localMapSizeX;
    int localMapSizeY;

    Vector2 zeroOffset;

    public int[,] localMap;
    Vector2 localZero;

    List<Vector2> buildCoordList;

    //debug. To be able to instantiate blocks
    //MapGenerator myGen;

    public SectionPositioner(int mapSizeX, int mapSizeY, int gapSize, MapGenerator Gen)
    {
        //myGen = Gen;
        localMapSizeX = mapSizeX;
        localMapSizeY = mapSizeY;

        localMap = new int[mapSizeX, mapSizeY];
        ClearLocalMap();

        Gap = gapSize;

        localZero = new Vector2(mapSizeX * 0.5f, 0);
        zeroOffset = new Vector2(((1 - MapGenerator.startSecionSize) * 0.5f), 0);

        buildCoordList = new List<Vector2>();

        AddPlace(localZero + zeroOffset);
    }



    public GlobalSectionCell DefineSection(int SectionId)
    {
        GlobalSectionCell definedSection = new GlobalSectionCell(Vector3.zero,0,0);

        if (SectionId == 1)
        {
            definedSection.Xsize = MapGenerator.startSecionSize;
            definedSection.Ysize = MapGenerator.startSecionSize;
        }
        //if (SectionId == 2)
        //{
        //    definedSection.Xsize = 8;
        //    definedSection.Ysize = 4;
        //}
        //if (SectionId == 3)
        //{
        //    definedSection.Xsize = 6;
        //    definedSection.Ysize = 3;
        //}
        //if (SectionId == 4)
        //{
        //    definedSection.Xsize = 5;
        //    definedSection.Ysize = 4;
        //}
        //if (SectionId == 5)
        //{
        //    definedSection.Xsize = 8;
        //    definedSection.Ysize = 7;
        //}
        //if (SectionId == 6)
        //{
        //    definedSection.Xsize = 5;
        //    definedSection.Ysize = 7;
        //}
        //if (SectionId == 7)
        //{
        //    definedSection.Xsize = 4;
        //    definedSection.Ysize = 8;
        //}
        //if (SectionId == 8)
        //{
        //    definedSection.Xsize = 4;
        //    definedSection.Ysize = 8;
        //}
        //if (SectionId == 9)
        //{
        //    definedSection.Xsize = 5;
        //    definedSection.Ysize = 6;
        //}
        //if (SectionId > 9)
        else
        {
            definedSection.Xsize = UnityEngine.Random.Range(MapGenerator.minSectionSize, MapGenerator.maxSectionSize + 1);
            definedSection.Ysize = UnityEngine.Random.Range(MapGenerator.minSectionSize, MapGenerator.maxSectionSize + 1);

            //definedSection.Xsize = 8;
            //definedSection.Ysize = 3;
        }

        Vector2 localNewPosition = FindNearestLocalPlace();
        MapCell localSectionPivot = new MapCell((int)localNewPosition.x, (int)localNewPosition.y);
    
        definedSection.Position = ConvertLocalToGlobal(localNewPosition);
        LocallSectionCell definedLocalSection = new LocallSectionCell(localSectionPivot, definedSection.Xsize, definedSection.Ysize);

        definedSection = BuildLocalSection(SectionId, definedLocalSection);

        ClearUpPlaces();

        return definedSection;
    }



    Vector2 FindNearestLocalPlace()
    {
        float shortestDistance = Mathf.Infinity;
        Vector2 nearestPlace = localZero;

        foreach (Vector2 place in buildCoordList)
        {
            float distanceToPlace = Vector2.Distance(localZero, place);

            if (distanceToPlace < shortestDistance)
            {
                shortestDistance = distanceToPlace;
                nearestPlace = place;                    
            } 
        }

        if (nearestPlace == localZero)
        {
            Debug.Log("AHTING!! No place to spawn section available");
        }

        RemovePlace(nearestPlace);

        return  nearestPlace;
    }

    

    GlobalSectionCell BuildLocalSection(int SectionId, LocallSectionCell section)   //<--- if gap > 1, there might be some problems.
    {
        //Fix pivot -> topleft
        //Fix X size
        //Fix Y size
        LocallSectionCell newSection = FindProperSection(section);

        //Populate localMap with id of the Section according to direction
        for (int i = 0; i < newSection.Xsize; i++)
        {
            for (int k = 0; k < newSection.Ysize; k++)
            {
                localMap[newSection.Position.X + i, newSection.Position.Y + k] = SectionId;
            }
        }

        //Don't forget to add perimeter
        MapCell focusCell = new MapCell(newSection.Position.X,newSection.Position.Y);
        focusCell.X--;
        focusCell.Y--;

        //Top Perimeter
        for (int i = 0; i < newSection.Xsize + Gap; i++)
        {
            if (IsInMapLimits(focusCell, EmptyId))
            {
                localMap[focusCell.X, focusCell.Y] = GapId;
            }

            focusCell.X++;
        }

        //Right Perimeter
        for (int i = 0; i < newSection.Ysize + Gap; i++)
        {
            if (IsInMapLimits(focusCell, EmptyId))
            {
                localMap[focusCell.X, focusCell.Y] = GapId;
            }

            focusCell.Y++;
        }

        //Bot Perimeter
        for (int i = 0; i < newSection.Xsize + Gap; i++)
        {
            if (IsInMapLimits(focusCell, EmptyId))
            {
                localMap[focusCell.X, focusCell.Y] = GapId;
            }

            focusCell.X--;
        }

        //Left Perimeter
        for (int i = 0; i < newSection.Ysize + Gap; i++)
        {
            if (IsInMapLimits(focusCell, EmptyId))
            {
                localMap[focusCell.X, focusCell.Y] = GapId;
            }

            focusCell.Y--;
        }

        //If this is the first section -> add spawn Vector on the right side;
        if (SectionId == 1)
        {
            Vector2 nextSectionPosition = new Vector2(newSection.Position.X + newSection.Xsize + Gap, newSection.Position.Y);
            AddPlace(nextSectionPosition);
        } else
        {
            CalculateNextSpawnPoints(newSection);
        }

        return ConvertLocalToGlobal(newSection);
    }



    void CalculateNextSpawnPoints(LocallSectionCell section)
    {
        Vector2 FirstSpawnPoint = Vector2.zero;
        Vector2 SecondSpawnPoint = Vector2.zero;

        List<MapCell> BlockPerimeter = TransormPerimeter(section);

        if (!IsInMapLimits(BlockPerimeter[0],EmptyId))
        {
            return;
        }

        FirstSpawnPoint = new Vector2(BlockPerimeter[0].X, BlockPerimeter[0].Y);

        for (int i = 1; i < BlockPerimeter.Count; i++)
        {
            if (!IsInMapLimits(BlockPerimeter[i], EmptyId))
            {
                SecondSpawnPoint = new Vector2(BlockPerimeter[i - 1].X, BlockPerimeter[i - 1].Y);
                break;
            }
        }

        if (FirstSpawnPoint != Vector2.zero)
        {
            AddPlace(FirstSpawnPoint);
        }

        if (SecondSpawnPoint != Vector2.zero)
        {
            AddPlace(SecondSpawnPoint);
        }
    }


 
    List<MapCell> TransormPerimeter(LocallSectionCell section)
    {
        List<MapCell> Perimeter = new List<MapCell>();
        MapCell focusCell = new MapCell(section.Position.X - Gap - 1, section.Position.Y - Gap - 1);
        int PerimeterShiftIndex = 0;

        //Go CLockWise -> add cell to the List
        //go Right
        for (int i = 0; i < (section.Xsize + Gap * 2 + 2); i++)
        {
            focusCell.X = section.Position.X - Gap - 1 + i;
            Perimeter.Add(focusCell);
        }

        //go Bot
        for (int i = 1; i < (section.Ysize + Gap * 2 + 2); i++)
        {
            focusCell.Y = section.Position.Y - Gap - 1 + i;
            Perimeter.Add(focusCell);
        }

        //go Left
        for (int i = 1; i < (section.Xsize + Gap * 2 + 2); i++)
        {
            focusCell.X = section.Position.X + section.Xsize + Gap - i;
            Perimeter.Add(focusCell);
        }

        //go Top
        for (int i = 1; i < (section.Ysize + Gap * 2 + 1); i++)
        {
            focusCell.Y = section.Position.Y + section.Ysize + Gap - i;
            Perimeter.Add(focusCell);
        }


        //First we need to find UNpropriate cell
        int searchIndex = 0;

        for (int i = 0; i < Perimeter.Count; i++)
        {
            if (!IsInMapLimits(Perimeter[i], EmptyId))
            {
                searchIndex = i;
                break;
            }
        }

        //Then we continue our search until we  find good Cell. In this place sometimes OutOfRangeExeption index appears...
        for (int i = searchIndex; i < Perimeter.Count; i++)
        {
            if (IsInMapLimits(Perimeter[i], EmptyId))
            {
                PerimeterShiftIndex = i;
                break;
            }
        }


        if (PerimeterShiftIndex == 0)
        {
            return Perimeter;
        }

        var ReorderedPerimeter = Perimeter.Skip(PerimeterShiftIndex).Concat(Perimeter.Take(PerimeterShiftIndex));
        var ReorderedList = ReorderedPerimeter.ToList<MapCell>();
        return ReorderedList;
    }



    public bool IsInMapLimits(MapCell cell)
    {
        if (cell.X >= 0 && cell.X <= localMapSizeX && cell.Y >= 0 && cell.Y <= localMapSizeY) return true;
        else return false;
    }
    bool IsInMapLimits(Vector2 place)
    {
        MapCell cell = new MapCell((int)place.x, (int)place.y);
        if (cell.X >= 0 && cell.X <= localMapSizeX && cell.Y >= 0 && cell.Y <= localMapSizeY) return true;
        else return false;
    }
    public bool IsInMapLimits(MapCell cell, int localCellId)
    {
        if (cell.X >= 0 && cell.X <= localMapSizeX && cell.Y >= 0 && cell.Y <= localMapSizeY)
        {
            if (localMap[cell.X,cell.Y] == localCellId)
            {
                return true;
            }
            else return false;
        }

        else return false;
    }

    //Check direction of the Section, and change Pivot to the top-left corner if needed
    LocallSectionCell FindProperSection(LocallSectionCell section)
    {
        //find proper pivot (left-top)
        //resize section, if needed

        //First, we need to define expand direction
        MapCell checkCell = new MapCell(section.Position.X,section.Position.Y);
        checkCell.X++;

        int Xdirection = 1;
        int Ydirection = 1;

        if (IsInMapLimits(checkCell,EmptyId))
        {
            Xdirection = 1;
        } else Xdirection = -1;

        checkCell.X = section.Position.X;
        checkCell.Y = section.Position.Y + 1;

        if (IsInMapLimits(checkCell,EmptyId))
        {
            Ydirection = 1;
        } else Ydirection = -1;

        //directions is determined
        //now we need to find new section position and size

        MapCell newLocalPos = new MapCell(section.Position.X,section.Position.Y);
        int newXsize = section.Xsize;
        int newYsize = section.Ysize;

        checkCell.X = newLocalPos.X;
        checkCell.Y = newLocalPos.Y;

        MapCell additionalCheckCell = new MapCell(checkCell.X, checkCell.Y);


        //Check X side
        if (Ydirection == 1)
        {
            additionalCheckCell.Y += (MapGenerator.minSectionSize - 1);
        } else
            additionalCheckCell.Y -= (MapGenerator.minSectionSize - 1);


        for (int i = 0; i < section.Xsize; i++)
        {
            if (!IsInMapLimits(checkCell, EmptyId) || !IsInMapLimits(additionalCheckCell, EmptyId))
            {
                newXsize = i;
                break;
            }

            if (Xdirection == 1)
            {
                checkCell.X++;
                additionalCheckCell.X++;
            } else
            {
                checkCell.X--;
                additionalCheckCell.X--;
            }
        }

        if (Xdirection == -1)
        {
            newLocalPos.X = checkCell.X + 1;
        }
        
        checkCell.X = newLocalPos.X;

        additionalCheckCell.X = section.Position.X;
        additionalCheckCell.Y = section.Position.Y;


        //Check Y side
        if (Xdirection == 1)
        {
            additionalCheckCell.X += (newXsize - 1);
        }

        for (int i = 0; i < section.Ysize; i++)
        {
            if (!IsInMapLimits(checkCell, EmptyId) || !IsInMapLimits(additionalCheckCell, EmptyId))
            {
                newYsize = i;
                break;
            }

            if (Ydirection == 1)
            {
                checkCell.Y++;
                additionalCheckCell.Y++;
            } else
            {
                checkCell.Y--;
                additionalCheckCell.Y--;
            }
        }

        if (Ydirection == -1)
        {
            newLocalPos.Y = checkCell.Y + 1;
        }


        if (localMap[newLocalPos.X,newLocalPos.Y] != EmptyId)
        {
            Debug.Log("AHTUNG: new pivot is occupied! " + newLocalPos);
        }

        return new LocallSectionCell(newLocalPos, newXsize, newYsize);
    }


    public MapCell ConvertGlobalToLocal(Vector3 globalCoord)
    {
        //Global (0,0,0) = local (97,0)
        //Global (2,0,-3) = local (99,3)
        int xCoord = (int)(globalCoord.x + localZero.x + zeroOffset.x);
        int yCoord = -(int)globalCoord.z;

        return new MapCell(xCoord, yCoord);
    }


    Vector3 ConvertLocalToGlobal(Vector2 localCoord)
    {
        //Local (0,0) = global (-100,0,0)
        //Local (99,3) = global (2,0,-3)
        //Local (93,3) = global (-4,0,-3)
        int xCoord = (int)(localCoord.x - localZero.x - zeroOffset.x);
        int zCoord = -(int)localCoord.y;

        return new Vector3(xCoord, 0, zCoord);
    }

    public Vector3 ConvertLocalToGlobal(MapCell localCoord)
    {
        int xCoord = (int)(localCoord.X - localZero.x - zeroOffset.x);
        int zCoord = -(int)localCoord.Y;

        return new Vector3(xCoord, 0, zCoord);
    }

    GlobalSectionCell ConvertLocalToGlobal(LocallSectionCell localSection)
    {
        Vector3 Pivot = ConvertLocalToGlobal(localSection.Position);

        return new GlobalSectionCell(Pivot,localSection.Xsize,localSection.Ysize);
    }

    void ClearLocalMap()
    {
        for (int i = 0; i < localMapSizeX; i++)
        {
            for (int y = 0; y < localMapSizeY; y++)
            {
                localMap[i, y] = EmptyId;
            }
        }
    }

    void AddPlace(Vector2 place)
    {
        if (IsInMapLimits(place) && !buildCoordList.Exists(x => x == place))
        {
            buildCoordList.Add(place);
            //myGen.SpawnPoint(ConvertLocalToGlobal(place));
            //Debug.Log("Added SpawnPoint: " + place);
        } else
        {
            Debug.Log("Duplicate place: " + place);
        }
    }

    void RemovePlace(Vector2 place)
    {
        buildCoordList.Remove(place);
        //Debug.Log("Remove SpawnPoint: " + place);
    }

    void ClearUpPlaces()
    {
        bool shouldRestart = false;

        //If place is not empty, remove it from List
        for (int i = 0; i < buildCoordList.Count; i++)
        {
            shouldRestart = false;
            Vector2 place = buildCoordList[i];
            MapCell placeCell = new MapCell((int)place.x, (int)place.y);
            
            if (!IsInMapLimits(placeCell, EmptyId))
            {
                RemovePlace(place);
                shouldRestart = true;
                break;
            }

            placeCell.X++;

            int Xdirection = 1;
            int Ydirection = 1;

            if (IsInMapLimits(placeCell, EmptyId))
            {
                Xdirection = 1;
            } else Xdirection = -1;

            placeCell.X = (int)place.x;
            placeCell.Y = (int)place.y + 1;

            if (IsInMapLimits(placeCell, EmptyId))
            {
                Ydirection = 1;
            } else Ydirection = -1;

            MapCell FirstCheckCell = new MapCell();         //Horizontal
            MapCell SecondCheckCell = new MapCell();        //Vectical
            MapCell ThirdCheckCell = new MapCell();         //Corner

            if (Xdirection == 1)
            {
                FirstCheckCell.X = (int)place.x + MapGenerator.minSectionSize - 1;
                SecondCheckCell.X = (int)place.x;
                ThirdCheckCell.X = FirstCheckCell.X;
            } else
            {
                FirstCheckCell.X = (int)place.x - MapGenerator.minSectionSize + 1;
                SecondCheckCell.X = (int)place.x;
                ThirdCheckCell.X = FirstCheckCell.X;
            }

            if (Ydirection == 1)
            {
                FirstCheckCell.Y = (int)place.y;
                SecondCheckCell.Y = (int)place.y + MapGenerator.minSectionSize - 1;
                ThirdCheckCell.Y = SecondCheckCell.Y;
            } else
            {
                FirstCheckCell.Y = (int)place.y;
                SecondCheckCell.Y = (int)place.y - MapGenerator.minSectionSize + 1;
                ThirdCheckCell.Y = SecondCheckCell.Y;
            }
 

            if (!IsInMapLimits(FirstCheckCell, EmptyId) || !IsInMapLimits(SecondCheckCell, EmptyId) || !IsInMapLimits(ThirdCheckCell, EmptyId))
            {
                RemovePlace(place);
                shouldRestart = true;
                break;
            }
        }

        if (shouldRestart)
        {
            ClearUpPlaces();
        }
    }
}
