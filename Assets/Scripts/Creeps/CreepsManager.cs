using System.Collections;
using UnityEngine;
using GameData;
using Zenject;

public class CreepsManager : ITickable {

    readonly Creep.Factory _creepFactory;
    readonly CreepsCollection _creepsCollection;
    readonly CreepWavesCollection _creepWavesCollection;

    private int DisplayWaveNum; //To display in UI
    private int WaveNum = 0; //Wave number from waves collection. Mayhap change later. Dependant on how we decided to spawn waves: predefined or choose from list and then additionaly modify
    private readonly float delayBetweenWaves = 3f;
    private float waveCountDown;
    private float waveCheckForCompletionCD = 1f;

    public SpawnState SpawnerState = SpawnState.PAUSE;

    public CreepsManager(   Creep.Factory creepFactory,
                            CreepsCollection creepsCollection,
                            CreepWavesCollection creepWavesCollection)
    {
        _creepFactory = creepFactory;
        _creepsCollection = creepsCollection;
        _creepWavesCollection = creepWavesCollection;
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

        for (int i = 0; i < currentWave.CreepAmount; i++)
        {
            SpawnCreep(currentWave.Creep);
            //wait for a delay
        }

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

    public void SpawnCreep(CreepData creepData)
    {
        var creep = _creepFactory.Create(creepData);
    }

}