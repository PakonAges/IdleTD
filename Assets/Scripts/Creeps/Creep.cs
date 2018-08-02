using UnityEngine;
using UnityEngine.AI;
using Zenject;

public class Creep : MonoBehaviour, ITargetable {

    private CreepData _creepData;

    private GlobalCreepPath _globalCreepPath;

    private CreepVisual _creepVisual;
    private CreepParameters _creepParameters;
    private CreepMovement _creepMovement;


    [Inject]
    public void Construct(GlobalCreepPath globalCreepPath)
    {
        _globalCreepPath = globalCreepPath;
    }

    private void Start()
    {
        _creepMovement.ResetMovement();
    }

    void Update()
    {
        if (Vector3.Distance(transform.position,_creepMovement.TargetToMove) <= 0.1f)
        {
            _creepMovement.GetNextWayPoint();
        }
    }

    private void Reset(CreepData creepData)
    {
        _creepData = creepData;
        _creepParameters = new CreepParameters(_creepData);
        _creepVisual = new CreepVisual(this, _creepData);
        _creepVisual.SetupVisual();


        var navAgent = this.gameObject.GetComponent<NavMeshAgent>();
        navAgent.enabled = false;
        _creepMovement = new CreepMovement(_creepParameters, _globalCreepPath, navAgent);
        _creepMovement.ResetMovement();
    }

    public bool IsAlive()
    {
        if (_creepParameters.HitPoints > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public class Pool : MonoMemoryPool<CreepData,  Creep>
    {
        protected override void Reinitialize(   CreepData creepData,
                                                Creep creep)
        {
            creep.Reset(creepData);
        }

        protected override void OnDespawned(Creep creep)
        {
            base.OnDespawned(creep);
            creep._creepVisual.SetOriginalScale();
        }
    }

}
