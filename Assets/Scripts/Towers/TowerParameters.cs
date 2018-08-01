public class TowerParameters
{
    private readonly TowerData _towerData;

    public int Damage;
    public float Range;
    public float AttackDelay;

    public TowerParameters(TowerData towerData)
    {
        _towerData = towerData;
        SetupTowerParameters();
    }

    private void SetupTowerParameters()
    {
        Damage = _towerData.Damage;
        Range = _towerData.Range;
        AttackDelay = _towerData.AttackDelay;
    }
}
