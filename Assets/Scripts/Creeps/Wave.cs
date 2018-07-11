using GameData;

public class Wave
{
    public int CreepCount { get; set; }
    public float SpawnRate { get; set; }

    public float HPmod { get; set; }
    public float RewardMod { get; set; }


    public Wave(WaveType type)
    {
        switch (type)
        {
            case (WaveType.Swarm):
                {
                    CreepCount = 20;
                    //CreepCount = 1;
                    SpawnRate = 0.5f;
                    HPmod = 1;
                    RewardMod = 1;
                    break;
                }
            case (WaveType.Normal):
                {
                    CreepCount = 10;
                    SpawnRate = 2.0f;
                    HPmod = 3;
                    RewardMod = 2;
                    break;
                }
            case (WaveType.BigBoys):
                {
                    CreepCount = 5;
                    SpawnRate = 5.0f;
                    HPmod = 10;
                    RewardMod = 4;
                    break;
                }
            case (WaveType.Boss):
                {
                    CreepCount = 1;
                    SpawnRate = 0.0f;
                    HPmod = 50;
                    RewardMod = 10;
                    break;
                }
        }
    }
}
