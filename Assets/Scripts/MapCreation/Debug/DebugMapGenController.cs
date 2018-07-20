using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class DebugMapGenController : MonoBehaviour {

    DebugMapGenerator _mapGenerator;
    //int x = 1;

    [Inject]
    public void Construct(DebugMapGenerator mapGenerator)
    {
        _mapGenerator = mapGenerator;
        _mapGenerator.deBugDrawSections = true;
        _mapGenerator.deBugDrawBridges = true;
        _mapGenerator.deBugDrawIndexMap = false;
    }

    private void Start()
    {
        Debug.Log("ARE YOU READY?");
    }

    void Update () {
        if (Input.GetKeyDown(KeyCode.G))
        {
            _mapGenerator.GenerateMap();
        }

        //if (Input.GetKeyDown(KeyCode.R))
        //{
        //    SceneManager.LoadScene("MapGenerator");
        //}
    }
}
