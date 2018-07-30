using System.Collections.Generic;
using System.Linq;
using Zenject;

public class WaveSpawner : ITickable
{
    readonly Creep.Pool _creepPool;
    readonly List<Creep> _creeps = new List<Creep>();
    readonly GlobalCreepPath _globalCreepPath;


    private CreepWave _wave;
    private readonly Dictionary<float,CreepData> _spawnTimeLine = new Dictionary<float, CreepData>();

    private bool _shouldSpawn;
    private float _spawnTimer = 0;
    private int _spawnedCreepsCounter = 0;

    public WaveSpawner( Creep.Pool creepPool,
                        GlobalCreepPath globalCreepPath)
    {
        _creepPool = creepPool;
        _globalCreepPath = globalCreepPath;
    }

    public void Tick()
    {
        if (_shouldSpawn)
        {
            //Not all creeps have been spawned
            if (_spawnedCreepsCounter < _wave.CreepAmount)
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
        _creeps.Add(_creepPool.Spawn(creepData, _globalCreepPath));
        _spawnedCreepsCounter++;
    }

    public void RemoveCreep()
    {
        var creep = _creeps[0];
        _creepPool.Despawn(creep);
        _creeps.Remove(creep);
    }
}
