using UnityEngine;
using GameData;
using Zenject;
using System.Collections.Generic;

public class CreepsManager : ITickable
{
    readonly CreepWavesCollection _creepWavesCollection;
    readonly WaveSpawner _waveSpawner;

    //Wave number To display in UI. Always incremented
    private int DisplayWaveNum = 0;

    //Wave number from waves collection. Mayhap change later. Dependant on how we decided to spawn waves: predefined or choose from list and then additionaly modify
    private int WaveNum = 0; 

    private readonly float delayBetweenWaves = 3f;
    private float waveCountDown;
    private float waveCheckForCompletionCD = 1f;

    public SpawnState SpawnerState = SpawnState.PAUSE;


    public CreepsManager(   CreepWavesCollection creepWavesCollection,
                            WaveSpawner waveSpawner)
    {
        _creepWavesCollection = creepWavesCollection;
        _waveSpawner = waveSpawner;
    }


    public void StartSpawningCreeps()
    {
        DisplayWaveNum = 1;
        waveCountDown = delayBetweenWaves;
        SpawnerState = SpawnState.COUNTDOWN;
    }


    public void Tick()
    {
        switch (SpawnerState)
        {
            case SpawnState.PAUSE:
            break;

            case SpawnState.SPAWNING:
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
        SpawnerState = SpawnState.WAITING;
    }


    bool WaveIsKilled()
    {
        waveCheckForCompletionCD -= Time.deltaTime;

        if (waveCheckForCompletionCD <= 0)
        {
            waveCheckForCompletionCD = 1f;

            if (_waveSpawner.IsAllCreepDead())
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
        DisplayWaveNum++;
        WaveNum++;
        SpawnerState = SpawnState.COUNTDOWN;
        waveCountDown = delayBetweenWaves;
    }
}