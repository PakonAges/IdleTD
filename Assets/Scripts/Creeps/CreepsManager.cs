using UnityEngine;
using GameData;
using Zenject;

public class CreepsManager : ITickable
{
    readonly CreepWavesCollection _creepWavesCollection;
    readonly WaveSpawner _waveSpawner;
    readonly SignalBus _signalBus;

    private IntVariable _displayWaveNum;

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

            if (_waveSpawner.AreAllCreepsDead())
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
}