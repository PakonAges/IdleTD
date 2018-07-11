using UnityEngine;
using UnityEngine.SceneManagement;

public class debugMapGenController : MonoBehaviour {

    public MapGenerator mapg;
    //int x = 1;

    private void Start()
    {
        Debug.Log("ARE YOU READY?");
    }

    void Update () {
        if (Input.GetKeyDown(KeyCode.G))
        {
            mapg.GenerateMap(30);
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
