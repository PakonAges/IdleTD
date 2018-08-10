using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

public class Creep : MonoBehaviour, ITargetable, IDisposable, IPoolable<CreepData, IMemoryPool>
{
    IMemoryPool _pool;
    private CreepData _creepData;

    private SignalBus _signalBus;
    private GlobalCreepPath _globalCreepPath;
    private List<Creep> _creepsAlive;

    //Meh. I don't realy like it here. I don't want to know how many creeps are alive here...
    private IntVariable _displayCurrentCreeps;

    private CreepVisual _creepVisual;
    private CreepParameters _creepParameters;
    private CreepMovement _creepMovement;
    bool _isAlive = false;

    [Inject]
    public void  Construct( GlobalCreepPath globalCreepPath,
                            WaveSpawner waveSpawner,
                            SignalBus signalBus,
                            PlayerData playerData)
    {
        _globalCreepPath = globalCreepPath;
        _creepsAlive = waveSpawner.CreepsAlive;
        _signalBus = signalBus;
        _displayCurrentCreeps = playerData.CurrentCreepsAlive.Variable;
    }

    public void Dispose()
    {
        _creepVisual.SetOriginalScale();
        _creepMovement.ResetMovement();
        _creepsAlive.Remove(this);
        _pool.Despawn(this);
        _displayCurrentCreeps.Value--;
        _signalBus.Fire<SignalCreepDied>();
    }

    public void OnSpawned(CreepData creepData, IMemoryPool pool)
    {
        _creepData = creepData;
        _pool = pool;
        _creepParameters = new CreepParameters(_creepData);
        _creepVisual = new CreepVisual(this, _creepData);
        _creepVisual.SetupVisual();

        var navAgent = gameObject.GetComponent<NavMeshAgent>();
        _creepMovement = new CreepMovement(_creepParameters, _globalCreepPath, navAgent);
        _creepMovement.StartMovement();

        _isAlive = true;
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
        Dispose();
    }

    public class Factory : PlaceholderFactory<CreepData, Creep>
    {

    }
}
