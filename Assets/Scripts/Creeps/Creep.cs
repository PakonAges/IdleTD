using System;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

public class Creep : MonoBehaviour, ITargetable, IDisposable, IPoolable<CreepData, IMemoryPool>
{
    IMemoryPool _pool;
    public CreepData CreepData;

    private SignalBus _signalBus;
    private GlobalCreepPath _globalCreepPath;

    private CreepVisual _creepVisual;
    private CreepParameters _creepParameters;
    private CreepMovement _creepMovement;
    bool _isAlive = false;

    [Inject]
    public void  Construct( GlobalCreepPath globalCreepPath,
                            SignalBus signalBus)
    {
        _globalCreepPath = globalCreepPath;
        _signalBus = signalBus;
    }

    public void Dispose()
    {
        _creepVisual.SetOriginalScale();
        _creepMovement.ResetMovement();
        _pool.Despawn(this);
    }

    public void OnSpawned(CreepData creepData, IMemoryPool pool)
    {
        CreepData = creepData;
        _pool = pool;
        _creepParameters = new CreepParameters(CreepData);
        _creepVisual = new CreepVisual(this, CreepData);
        _creepVisual.SetupVisual();

        var navAgent = gameObject.GetComponent<NavMeshAgent>();
        _creepMovement = new CreepMovement(_creepParameters, _globalCreepPath, navAgent);
        _creepMovement.StartMovement();

        _isAlive = true;
    }

    public void OnDespawned()
    {
        _pool = null;
        CreepData = null;
        _creepVisual = null;
        _creepParameters = null;
        _creepMovement = null;

        _isAlive = false;
    }

    void Update()
    {
        if (_isAlive)
        {
            if (Vector3.Distance(transform.position, _creepMovement.TargetToMove) <= 0.1f)
            {
                _creepMovement.GetNextWayPoint();
            }
        }
    }

    public bool IsAlive()
    {
        if (_isAlive)
        {
            return true;
        }
        else return false;
    }

    public void TakeDamage(int dmg)
    {
        _creepParameters.CurrentHitPoints -= dmg;

        if (_creepParameters.CurrentHitPoints <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        _creepParameters.CurrentHitPoints = 0;
        _signalBus.Fire(new SignalCreepDied(this));
        Dispose();
    }

    public class Factory : PlaceholderFactory<CreepData, Creep>
    {

    }
}
