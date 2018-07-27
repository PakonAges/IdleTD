using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Setup Move parameters, Get Movement Targets
/// </summary>
public class CreepMovement {

    private readonly GlobalCreepPath _creepPath;
    public NavMeshAgent Agent;

    private readonly float moveSpeed;
    //private Vector3 TargetToMove;
    public Vector3 TargetToMove { get; set; }
    Vector3 prevTarget;


    public CreepMovement(   CreepData creepData,
                            GlobalCreepPath globalCreepPath,
                            NavMeshAgent navMeshAgent)
    {
        _creepPath = globalCreepPath;
        Agent = navMeshAgent;
        Agent.speed = creepData.MoveSpeed;
    }

    public void ResetMovement()
    {
        prevTarget = _creepPath.Path[0];
        TargetToMove = _creepPath.Path[1];

        Agent.Warp(prevTarget);
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
