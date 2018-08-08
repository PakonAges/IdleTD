using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Setup Move parameters, Get Movement Targets
/// </summary>
public class CreepMovement {

    private readonly GlobalCreepPath _creepPath;
    public NavMeshAgent Agent;

    //private readonly float moveSpeed;
    public Vector3 TargetToMove { get; set; }
    Vector3 prevTarget;


    public CreepMovement(   CreepParameters creepParameters,
                            GlobalCreepPath globalCreepPath,
                            NavMeshAgent navMeshAgent)
    {
        _creepPath = globalCreepPath;
        Agent = navMeshAgent;
        Agent.speed = creepParameters.MoveSpeed;
        ResetMovement();
    }

    public void ResetMovement()
    {
        Agent.enabled = true;
        prevTarget = _creepPath.Path[0];
        TargetToMove = _creepPath.Path[1];

        Agent.Warp(prevTarget);
        Agent.gameObject.transform.LookAt(TargetToMove);
        Agent.enabled = false;
    }

    public void StartMovement()
    {
        Agent.enabled = true;
        Agent.SetDestination(TargetToMove);
    }

    public void GetNextWayPoint()
    {
        Vector3 newTarget = _creepPath.GetNextWp(prevTarget, TargetToMove);
        prevTarget = TargetToMove;
        TargetToMove = newTarget;
        Agent.SetDestination(TargetToMove);
    }
}
