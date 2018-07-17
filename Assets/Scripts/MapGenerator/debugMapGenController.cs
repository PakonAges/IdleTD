using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class DebugMapGenController : MonoBehaviour {

    MapGenerator _mapGenerator;
    //int x = 1;

    [Inject]
    public void Construct(MapGenerator mapGenerator)
    {
        _mapGenerator = mapGenerator;
    }

    private void Start()
    {
        Debug.Log("ARE YOU READY?");
    }

    void Update () {
        if (Input.GetKeyDown(KeyCode.G))
        {
            _mapGenerator.GenerateMap(5);
        }

        //if (Input.GetKeyDown(KeyCode.G))
        //{
        //    mapg.GenerateMap(x);
        //    x++;
        //}

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("MapGenerator");
        }
    }
}
