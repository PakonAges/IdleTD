using UnityEngine;
using Zenject;

public class Tower : MonoBehaviour
{
    public TowerParameters TowerParameters;

    private TowerData _towerData;
    private TowerVisual _towerVisual;
    private TowerTargeting _towerTargeting;
    private TowerShooting _towerShooting;

    [Inject]
    public void Construct(  Vector3 position,
                            TowerData towerData,
                            WaveSpawner waveSpawner,
                            Bullet.Pool bulletPool)
    {
        _towerData = towerData;
        TowerParameters = new TowerParameters(_towerData);
        _towerVisual = new TowerVisual(this, _towerData);
        _towerTargeting = new TowerTargeting(this, waveSpawner);
        _towerShooting = new TowerShooting(bulletPool);

        gameObject.transform.position = position;
    }



    private void LateUpdate()
    {
        _towerTargeting.ManageTarget();

        if (_towerTargeting.Target != null)
        {
            _towerShooting.ManageShooting();
        }
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
