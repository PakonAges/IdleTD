using UnityEngine;
using GameData;
using Zenject;
using System;
using System.Collections.Generic;
using System.Linq;

public class CreepsManager : ITickable, IInitializable, IDisposable
{
    public readonly List<Creep> CreepsAlive = new List<Creep>();

    readonly CreepWavesCollection _creepWavesCollection;
    readonly WaveSpawner _waveSpawner;
    readonly SignalBus _signalBus;

    private IntVariable _displayWaveNum;
    private IntVariable _displayCurrentCreeps;

    //Wave number from waves collection. Mayhap change later. Dependant on how we decided to spawn waves: predefined or choose from list and then additionaly modify
    private int WaveNum = 0; 

    private readonly float delayBetweenWaves = 3f;
    private float waveCountDown;
    private float waveCheckForCompletionCD = 1f;
    public SpawnState SpawnerState = SpawnState.PAUSE;

    public CreepsManager(   CreepWavesCollection creepWavesCollection,
                            WaveSpawner waveSpawner,
                            SignalBus signalBus,
                            PlayerData playerData)
    {
        _creepWavesCollection = creepWavesCollection;
        _waveSpawner = waveSpawner;
        _signalBus = signalBus;
        _displayWaveNum = playerData.CurrentWave.Variable;
        _displayCurrentCreeps = playerData.CurrentCreepsAlive.Variable;
    }

    public void Initialize()
    {
        _signalBus.Subscribe<SignalCreepSpawned>(OnCreepSpawned);
        _signalBus.Subscribe<SignalCreepDied>(OnCreepDied);

        //Reset Creeps Counter
        _displayCurrentCreeps.Value = 0;
    }


    public void Dispose()
    {
        _signalBus.Unsubscribe<SignalCreepSpawned>(OnCreepSpawned);
        _signalBus.Unsubscribe<SignalCreepDied>(OnCreepDied);
    }


    private void OnCreepSpawned(SignalCreepSpawned args)
    {
        _displayCurrentCreeps.Value++;
        CreepsAlive.Add(args.Creep);
        _signalBus.Fire(new SignalCreepsCounterChanged());
    }

    private void OnCreepDied(SignalCreepDied args)
    {
        _displayCurrentCreeps.Value--;
        CreepsAlive.Remove(args.Creep);
        _signalBus.Fire(new SignalCreepsCounterChanged());
    }

    public void StartSpawningCreeps()
    {
        _displayWaveNum.Value = 1;
        waveCountDown = delayBetweenWaves;
        SpawnerState = SpawnState.COUNTDOWN;
        _signalBus.Fire<SignalNewWave>();
    }


    public void Tick()
    {
        switch (SpawnerState)
        {
            case SpawnState.PAUSE:
            break;

            case SpawnState.SPAWNING:
            if (_waveSpawner.AreAllCreepsSpawned())
            {
                SpawnerState = SpawnState.WAITING;
            }
            break;

            case SpawnState.WAITING:
            if (WaveIsKilled())
            {
                WaveCompleted();
                return;
            }
            break;

            case SpawnState.COUNTDOWN:
            if (waveCountDown <= 0)
            {
                SpawnerState = SpawnState.SPAWNING;
                SpawnWave();
            }
            else
            {
                waveCountDown -= Time.deltaTime;
            }

            return;
        }
    }


    void SpawnWave()
    {
        if (WaveNum >= _creepWavesCollection.CreepWaves.Count)
        {
            WaveNum = 0;
        }

        var currentWave = _creepWavesCollection.CreepWaves[WaveNum];
        _waveSpawner.SpawnWave(currentWave);
    }


    bool WaveIsKilled()
    {
        waveCheckForCompletionCD -= Time.deltaTime;

        if (waveCheckForCompletionCD <= 0)
        {
            waveCheckForCompletionCD = 1f;

            if (AreAllCreepsDead())
            {
                return true;
            }
            else return false;
        }
        else return false;
    }


    void WaveCompleted()
    {
        //Next Wave indexes
        _displayWaveNum.Value++;
        WaveNum++;
        SpawnerState = SpawnState.COUNTDOWN;
        waveCountDown = delayBetweenWaves;
        _signalBus.Fire<SignalNewWave>();
    }

    public void RemoveFirstCreep()
    {
        if (CreepsAlive.Any())
        {
            var creep = CreepsAlive[0];
            creep.Dispose();
        }
    }

    public bool AreAllCreepsDead()
    {
        if (CreepsAlive.Count > 0)
        {
            return false;
        }
        else return true;
    }
}