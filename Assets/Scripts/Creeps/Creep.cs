using System;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

public class Creep : MonoBehaviour, ITargetable, IDisposable, IPoolable<CreepData, IMemoryPool>
{
    IMemoryPool _pool;
    private CreepData _creepData;

    private GlobalCreepPath _globalCreepPath;

    private CreepVisual _creepVisual;
    private CreepParameters _creepParameters;
    private CreepMovement _creepMovement;
    bool _isAlive = false;

    [Inject]
    public void  Construct(GlobalCreepPath globalCreepPath)
    {
        _globalCreepPath = globalCreepPath;
    }

    public void OnSpawned(CreepData creepData, IMemoryPool pool)
    {
        _creepData = creepData;
        _pool = pool;
        _creepParameters = new CreepParameters(_creepData);
        _creepVisual = new CreepVisual(this, _creepData);
        _creepVisual.SetupVisual();

        var navAgent = this.gameObject.GetComponent<NavMeshAgent>();
        navAgent.enabled = false;
        _creepMovement = new CreepMovement(_creepParameters, _globalCreepPath, navAgent);
        _creepMovement.ResetMovement();
        _isAlive = true;
    }

    public void Dispose()
    {
        //Remove from WaveSpawner List<Creep> _CreepsAlive, so we would know that all creeps have died;
        _pool.Despawn(this);
    }

    public void OnDespawned()
    {
        _pool = null;
        _creepData = null;
        _creepVisual = null;
        _creepParameters = null;
        _creepMovement = null;
        _isAlive = false;
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, _creepMovement.TargetToMove) <= 0.1f)
        {
            _creepMovement.GetNextWayPoint();
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
        Dispose();
    }

    public class Factory : PlaceholderFactory<CreepData, Creep>
    {

    }
}
