using UnityEngine.AI;
using UnityEngine;

public class NavMeshCreator : MonoBehaviour {

    public GameObject NavMesh;

    public void GenerateNavMesh()
    {
        //Check for null in NavMesh refference
        NavMesh.GetComponent<NavMeshSurface>().BuildNavMesh();
    }

}
