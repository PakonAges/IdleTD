﻿using System.Collections.Generic;
using System.Linq;
using Zenject;

public class WaveSpawner : ITickable
{
    readonly Creep.Factory _creepFactory;
    public readonly List<Creep> CreepsAlive = new List<Creep>();

    private CreepWave _wave;
    private readonly Dictionary<float,CreepData> _spawnTimeLine = new Dictionary<float, CreepData>();

    private bool _shouldSpawn;
    private float _spawnTimer = 0;
    private int _spawnedCreepsCounter = 0;

    public WaveSpawner( Creep.Factory creepFactory)
    {
        _creepFactory = creepFactory;
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
        CreepsAlive.Add(_creepFactory.Create(creepData));
        _spawnedCreepsCounter++;
    }

    public void RemoveCreep()
    {
        if (CreepsAlive.Any())
        {
            var creep = CreepsAlive[0];
            creep.Dispose();
        }
    }

    public bool IsAllCreepDead()
    {
        if (CreepsAlive.Count > 0)
        {
            return false;
        }
        else return true;
    }
}
