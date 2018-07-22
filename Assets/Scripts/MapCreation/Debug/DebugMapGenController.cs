using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class DebugMapGenController : MonoBehaviour {

    IMapGenerator _mapGenerator;

    [Inject]
    public void Construct(IMapGenerator mapGenerator)
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
            _mapGenerator.GenerateMap();
        }

        //if (Input.GetKeyDown(KeyCode.R))
        //{
        //    SceneManager.LoadScene("MapGenerator");
        //}
    }
}
