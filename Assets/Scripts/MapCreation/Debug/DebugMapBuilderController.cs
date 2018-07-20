using UnityEngine;
using Zenject;

public class DebugMapBuilderController : MonoBehaviour
{
    public MapBuilder MapBuilder;

    private void Start()
    {
        Debug.Log("MAP BUILDER INITIATED");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            MapBuilder.BuildMap();
        }
    }
}
