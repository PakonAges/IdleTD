namespace GameData
{
    public enum Side
    {
        None,
        Top,
        Right,
        Bot,
        Left
    }

    public enum TileType
    {
        Empty,
        Ground,
        Road,
        Bridge,
        Entrance,
        Exit
    }

    public enum TowerType
    {
        Normal,
        AoE,
        Lazer
    }

    public enum CreepType
    {
        Normal
    }

    public enum WaveType
    {
        Swarm,
        Normal,
        BigBoys,
        Boss
    }

    //Wave spawner states
    public enum SpawnState
    {
        SPAWNING,
        WAITING,
        COUNTING,
    };
}
