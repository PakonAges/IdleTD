using UnityEngine;
using Zenject;

public class Tower : MonoBehaviour
{
    public TowerParameters TowerParameters;

    public TowerData TowerData;
    private TowerVisual _towerVisual;
    private TowerTargeting _towerTargeting;
    private TowerShooting _towerShooting;

    [Inject]
    public void Construct(  Vector3 position,
                            TowerData towerData,
                            WaveSpawner waveSpawner,
                            Bullet.Pool bulletPool)
    {
        TowerData = towerData;
        TowerParameters = new TowerParameters(TowerData);
        _towerVisual = new TowerVisual(this, TowerData);
        _towerTargeting = new TowerTargeting(this, waveSpawner);
        _towerShooting = new TowerShooting(this, bulletPool);

        gameObject.transform.position = position;
        _towerVisual.SetupVisual();
    }

    public Transform Target()
    {
        return _towerTargeting.Target;
    }

    public void TargetIsUnavailable()
    {
        _towerShooting.TowerShootingState = TowerShooting.ShootingState.NoTarget;
    }


    private void LateUpdate()
    {
        _towerTargeting.ManageTarget();
        _towerShooting.ManageShooting();
    }


    public class Factory : PlaceholderFactory<Vector3, TowerData, Tower> { }


    private void OnDrawGizmos()
    {
        if (_towerTargeting.Target != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, _towerTargeting.Target.position);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, TowerParameters.Range);
    }
}
