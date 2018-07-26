using UnityEngine;
using UnityEngine.AI;
using Zenject;

public class Creep : MonoBehaviour {

    private CreepData _creepData;

    private CreepVisual _creepVisual;
    private CreepParameters _creepParameters;
    private CreepMovement _creepMovement;


    [Inject]
    public void Construct(  CreepData creepData,
                            GlobalCreepPath globalCreepPath)
    {
        _creepData = creepData;
        _creepVisual = new CreepVisual(this, _creepData);
        _creepParameters = new CreepParameters(_creepData);
        _creepMovement = new CreepMovement(_creepData, globalCreepPath, this.gameObject.GetComponentInChildren<NavMeshAgent>());

        gameObject.transform.position = _creepMovement.GetSpawnPosition();
    }

    void Update()
    {
        if (Vector3.Distance(transform.position,_creepMovement.TargetToMove) <= 0.1f)
        {
            _creepMovement.GetNextWayPoint();
        }
    }

    public class Factory : PlaceholderFactory<CreepData, Creep> { }

}
