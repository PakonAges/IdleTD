using UnityEngine;
using UnityEngine.AI;

public class CreepMovement : MonoBehaviour {

    float moveSpeed = 5.0f;

    Vector3 targetToMove;
    Vector3 prevTarget;    

    // Use this for initialization
    void Start() {
        ResetMovement();
        GetComponent<NavMeshAgent>().speed = moveSpeed;
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 dir = targetToMove - transform.position;
        transform.Translate(dir.normalized * moveSpeed * Time.deltaTime, Space.World);
        //transform.rotation = Quaternion.LookRotation(dir);

        if (Vector3.Distance(transform.position,targetToMove) <= 0.1f)
        {
            GetNextWayPoint();
        }
	}

    public void ResetMovement()
    {
        //prevTarget = CreepPath.instance.path[0];
        //targetToMove = CreepPath.instance.path[1];
    }

    void GetNextWayPoint()
    {
        //Vector3 newTarget = CreepPath.instance.GetNextWp(prevTarget, targetToMove);
        prevTarget = targetToMove;
        //targetToMove = newTarget;

        //Vector3 dir = targetToMove - transform.position;
        //transform.rotation = Quaternion.LookRotation(dir);
    }
}
