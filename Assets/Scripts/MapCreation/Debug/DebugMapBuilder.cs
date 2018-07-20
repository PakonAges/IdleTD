using UnityEngine;

public class DebugMapBuilder : MapBuilder
{
    private void Start()
    {
        Debug.Log("Map Builder Initialized");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            //BuildMap();
            //Take map from generator
            //Take portals from map
        }
    }
}
