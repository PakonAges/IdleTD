using System.Collections.Generic;
using UnityEngine;

public class CreepPath {

    static CreepPath creepPath;
    public static CreepPath instance
    {
        get
        {
            if (creepPath == null)
            {
                creepPath = new CreepPath();
            }

            return creepPath;
        }
    }

    public List<Vector3> path = new List<Vector3>();

    public void AddPath(Vector3 wp)
    {
        path.Add(wp);
    }

    public void AddPath(List<Vector3> ListOfWp)
    {
        path.AddRange(ListOfWp);
    }

    public void AddPath(int index, List<Vector3> ListOfWp)
    {
        path.InsertRange(index,ListOfWp);
    }

    public Vector3 GetNextWp(Vector3 prevWp, Vector3 currentTarget)
    {
        int prevIndex = path.IndexOf(prevWp);
        int currentIndex = path.IndexOf(currentTarget);

        //last wp of the path
        if (currentIndex == path.Count - 1)
        {
            //repeat or exit in the future!
            return path[1];
        }

        if (prevIndex == path.Count - 1)
        {
            return path[2];
        }

        //direct path
        if (prevIndex == currentIndex - 1)
        {
            return path[currentIndex + 1];
        }

        //returning point
        else
        {
            //loop through all path, until find 2 wp in sequence
            for (int i = 0; i < path.Count - 2; i++)
            {
                if (path[i] == prevWp && path[i + 1] == currentTarget)
                {
                    return path[i + 2];
                }
            }
            return Vector3.zero;
        }        
    }

}
