using UnityEngine;
using Zenject;


/// <summary>
/// Used as Input Point to manipulate Generated local Map Data
/// </summary>
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
            Debug.Log("Map Generated!");
        }
    }
}
