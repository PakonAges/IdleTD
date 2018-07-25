using System.Collections.Generic;
using UnityEngine;

public class GlobalCreepPath {

    //static CreepPath creepPath;
    //public static CreepPath instance
    //{
    //    get
    //    {
    //        if (creepPath == null)
    //        {
    //            creepPath = new CreepPath();
    //        }

    //        return creepPath;
    //    }
    //}

    public List<Vector3> Path = new List<Vector3>();

    public void AddPath(Vector3 wp)
    {
        Path.Add(wp);
    }

    public void AddPath(List<Vector3> ListOfWp)
    {
        Path.AddRange(ListOfWp);
    }

    public void AddPath(int index, List<Vector3> ListOfWp)
    {
        Path.InsertRange(index,ListOfWp);
    }

    public Vector3 GetNextWp(Vector3 prevWp, Vector3 currentTarget)
    {
        int prevIndex = Path.IndexOf(prevWp);
        int currentIndex = Path.IndexOf(currentTarget);

        //last wp of the path
        if (currentIndex == Path.Count - 1)
        {
            //repeat or exit in the future!
            return Path[1];
        }

        if (prevIndex == Path.Count - 1)
        {
            return Path[2];
        }

        //direct path
        if (prevIndex == currentIndex - 1)
        {
            return Path[currentIndex + 1];
        }

        //returning point
        else
        {
            //loop through all path, until find 2 wp in sequence
            for (int i = 0; i < Path.Count - 2; i++)
            {
                if (Path[i] == prevWp && Path[i + 1] == currentTarget)
                {
                    return Path[i + 2];
                }
            }
            return Vector3.zero;
        }        
    }

}
