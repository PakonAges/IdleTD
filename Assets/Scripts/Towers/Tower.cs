using UnityEngine;
using Zenject;

public class Tower : MonoBehaviour
{
    private TowerData _towerData;

    private TowerVisual _towerVisual;
    public TowerParameters TowerParameters;
    private TowerTargeting _towerTargeting;

    //Array of Details

    [Inject]
    public void Construct(  Vector3 position,
                            TowerData towerData,
                            WaveSpawner waveSpawner)
    {
        _towerData = towerData;
        TowerParameters = new TowerParameters(_towerData);
        _towerVisual = new TowerVisual(this, _towerData);
        _towerTargeting = new TowerTargeting(this, waveSpawner);

        gameObject.transform.position = position;
    }

    private void LateUpdate()
    {
        _towerTargeting.ManageTarget();
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
