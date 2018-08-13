public struct SignalNewWave { }
public struct SignalCreepsCounterChanged { }

public struct SignalCreepSpawned
{
    public SignalCreepSpawned(Creep creep)
    {
        Creep = creep;
    }

    public Creep Creep { get; private set; }
}

public struct SignalCreepDied
{
    public SignalCreepDied(Creep creep)
    {
        Creep = creep;
    }

    public Creep Creep { get; private set; }
}