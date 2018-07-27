using UnityEngine;
using GameData;
using Zenject;

public class CreepsManager : ITickable {
    readonly CreepsCollection _creepsCollection;
    readonly CreepWavesCollection _creepWavesCollection;
    readonly WaveSpawner _waveSpawner;

    private int DisplayWaveNum; //To display in UI
    private int WaveNum = 0; //Wave number from waves collection. Mayhap change later. Dependant on how we decided to spawn waves: predefined or choose from list and then additionaly modify
    private readonly float delayBetweenWaves = 3f;
    private float waveCountDown;
    private float waveCheckForCompletionCD = 1f;

    public SpawnState SpawnerState = SpawnState.PAUSE;

    public CreepsManager(   CreepsCollection creepsCollection,
                            CreepWavesCollection creepWavesCollection,
                            WaveSpawner waveSpawner)
    {
        _creepsCollection = creepsCollection;
        _creepWavesCollection = creepWavesCollection;
        _waveSpawner = waveSpawner;
    }

    public void StartSpawningCreeps()
    {
        DisplayWaveNum = WaveNum + 1;
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

            return;
        }

        if (waveCountDown <= 0)
        {
            if (SpawnerState != SpawnState.SPAWNING)
            {
                SpawnerState = SpawnState.SPAWNING;
                SpawnWave();
            }
        }
        else
        {
            waveCountDown -= Time.deltaTime;
        }
    }


    void SpawnWave()
    {
        if (WaveNum > _creepWavesCollection.CreepWaves.Count)
        {
            WaveNum = 0;
        }

        var currentWave = _creepWavesCollection.CreepWaves[WaveNum];
        _waveSpawner.SpawnWave(currentWave);
        SpawnerState = SpawnState.WAITING;
    }


    bool WaveIsKilled()
    {
        return false; //WE will change pooler, so we need to also change where we check for alive creeps

        //waveCheckForCompletionCD -= Time.deltaTime;

        //if (waveCheckForCompletionCD <= 0)
        //{
        //    waveCheckForCompletionCD = 1f;

        //    for (int i = 0; i < CreepsPooler.current.Pool.Count; i++)
        //    {
        //        if (CreepsPooler.current.Pool[i] != null && CreepsPooler.current.Pool[i].activeInHierarchy)
        //        {
        //            return true;
        //        }
        //    }
        //    return false;
        //} else return true;
    }

    void WaveCompleted()
    {
        SpawnerState = SpawnState.COUNTDOWN;
        waveCountDown = delayBetweenWaves;
        DisplayWaveNum++;
        WaveNum++;
    }
}