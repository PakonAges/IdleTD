using System.Collections.Generic;
using System.Linq;
using Zenject;

public class WaveSpawner : ITickable
{
    readonly SignalBus _signalBus;
    readonly Creep.Factory _creepFactory;

    private CreepWave _wave;
    private readonly Dictionary<float,CreepData> _spawnTimeLine = new Dictionary<float, CreepData>();

    private bool _shouldSpawn;
    private float _spawnTimer = 0;
    private int _spawnedCreepsCounter = 0;

    public WaveSpawner( Creep.Factory creepFactory,
                        SignalBus signalBus)
    {
        _creepFactory = creepFactory;
        _signalBus = signalBus;
    }

    public void Tick()
    {
        if (_shouldSpawn)
        {
            if (!AreAllCreepsSpawned())
            {
                if (_spawnTimer >= _spawnTimeLine.Keys.ElementAt(_spawnedCreepsCounter))
                {
                    AddCreep(_wave.Creep);
                }

                _spawnTimer += UnityEngine.Time.deltaTime;
            }
            //Spawned all creeps
            else
            {
                _shouldSpawn = false;
            }
        }
    }

    public void SpawnWave(CreepWave wave)
    {
        _wave = wave;
        ResetTimers();
        CreateSpawnTimeLine();
        _shouldSpawn = true;
    }

    private void CreateSpawnTimeLine()
    {
        for (int i = 0; i < _wave.CreepAmount; i++)
        {
            _spawnTimeLine.Add(_wave.SpawnRate*i, _wave.Creep);
        }
    } 

    private void ResetTimers()
    {
        _spawnTimeLine.Clear();
        _spawnTimer = 0;
        _spawnedCreepsCounter = 0;
    }

    public void AddCreep(CreepData creepData)
    {
        var newCreep = _creepFactory.Create(creepData);
        _spawnedCreepsCounter++;
        _signalBus.Fire(new SignalCreepSpawned(newCreep));
    }

    public bool AreAllCreepsSpawned()
    {
        if (_spawnedCreepsCounter < _wave.CreepAmount)
        {
            return false;
        }
        else return true;
    }
}
