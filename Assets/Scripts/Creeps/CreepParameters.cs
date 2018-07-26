public class CreepParameters {
    private readonly CreepData _creepData;

    public float MoveSpeed;
    public int HitPoints;
    public int Reward;

    public CreepParameters(CreepData creepData)
    {
        _creepData = creepData;

        SetupParameters();
    }

    private void SetupParameters()
    {
        MoveSpeed = _creepData.MoveSpeed;
        HitPoints = _creepData.HitPoints;
        Reward = _creepData.Reward;
    }
}
