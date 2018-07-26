using System.Collections;
using UnityEngine;
using GameData;
using Zenject;

public class CreepsManager : ITickable {

    readonly Creep.Factory _creepFactory;
    readonly CreepsCollection _creepsCollection;

    //debug
    private bool Spawn = true;

    int WaveNum = 1;
    readonly float delayBetweenWaves = 3f;
    float waveCountDown;

    float searchCountdown = 1f;

    public SpawnState SpawnerState = SpawnState.PAUSE;

    public CreepsManager(   Creep.Factory creepFactory,
                            CreepsCollection creepsCollection)
    {
        _creepFactory = creepFactory;
        _creepsCollection = creepsCollection;
        //WaveNum = PlayerStats.instance.WaveNumber;
    }

    public void StartSpawningCreeps()
    {
        waveCountDown = delayBetweenWaves;
        SpawnerState = SpawnState.COUNTDOWN;
    }

    public void SpawnCreep()
    {
        var creep = _creepFactory.Create(_creepsCollection.CreepsList[0]);
    }

    public void Tick()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            SpawnCreep();
        }

        if (Spawn)
        {
            //var creep = _creepFactory.Create(_creepsCollection.CreepsList[0]);
            //var creep2 = _creepFactory.Create(_creepsCollection.CreepsList[1]);
            Spawn = false;
        }

        switch (SpawnerState)
        {
            case SpawnState.WAITING:
            if (!WaveIsAlive())
            {
                WaveCompleted();
                return;
            }
            else
            {
                return;
            }
        }

        if (waveCountDown <= 0)
        {
            if (SpawnerState != SpawnState.SPAWNING)
            {
                //StartCoroutine(SpawnWave(GenerateWave(WaveNum)));
            }
        }
        else
        {
            waveCountDown -= Time.deltaTime;
        }
    }



    Wave GenerateWave(int waveNumber)
    {
        WaveType type = WaveType.Swarm;

        if (waveNumber % 10 == 0)
        {
            type = WaveType.Normal;
        } else

        if (waveNumber % 5 == 0)
        {
            type = WaveType.BigBoys;
        } else

        if (waveNumber % 3 == 0)
        {
            type = WaveType.Boss;
        }

        Wave newWave = new Wave(type);

        return newWave;
    }




    IEnumerator SpawnWave(Wave _wave)
    {
        SpawnerState = SpawnState.SPAWNING;

        for (int i = 0; i < _wave.CreepCount; i++)
        {
            //SpawnCreep(CreepType.Normal,(int)(PlayerStats.instance.WaveNumber*_wave.HPmod), (int)(PlayerStats.instance.WaveNumber*_wave.RewardMod));
            SpawnCreep(CreepType.Normal, 100, 10);
            yield return new WaitForSeconds(_wave.SpawnRate);
        }

        SpawnerState = SpawnState.WAITING;

        yield break;
    }



    void SpawnCreep(CreepType type, int HP, int CoinsReward)
    {
        //GameObject creep = CreepsPooler.current.GetPooledObject();
        //creep.transform.position = CreepPath.instance.path[0];
        //creep.GetComponent<CreepMain>().Init(HP, CoinsReward);
        //creep.SetActive(true);
        //EventManager.Broadcast(gameEvent.CreepSpawned, new eventArgExtend() { creep = creep.GetComponent<CreepMain>() });
    }



    bool WaveIsAlive()
    {
        searchCountdown -= Time.deltaTime;
        if (searchCountdown <= 0)
        {
            searchCountdown = 1f;

            for (int i = 0; i < CreepsPooler.current.Pool.Count; i++)
            {
                if (CreepsPooler.current.Pool[i] != null && CreepsPooler.current.Pool[i].activeInHierarchy)
                {
                    return true;
                }
            }
            return false;
        } else return true;
    }



    void WaveCompleted()
    {
        SpawnerState = SpawnState.COUNTDOWN;
        waveCountDown = delayBetweenWaves;

        WaveNum++;
        //PlayerStats.instance.WaveNumber++;

        //EventManager.Broadcast(gameEvent.WaveCompleted, new eventArgExtend());
    }


}