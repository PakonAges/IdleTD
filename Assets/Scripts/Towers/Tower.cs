﻿using UnityEngine;
using Zenject;

public class Tower : MonoBehaviour
{
    //public Tile tile {get; private set;}

    //public void SetTile(Tile tile){
    // place tower on the tile
    //}


    public TowerParameters TowerParameters;

    public TowerData TowerData;
    private TowerVisual _towerVisual;
    private TowerTargeting _towerTargeting;
    private TowerShooting _towerShooting;

    [Inject]
    public void Construct(  Vector3 position,
                            TowerData towerData,
                            CreepsManager creepsManager,
                            Bullet.Factory bulletFactory)
    {
        TowerData = towerData;
        TowerParameters = new TowerParameters(TowerData);
        _towerVisual = new TowerVisual(this, TowerData);
        _towerTargeting = new TowerTargeting(this, creepsManager);
        _towerShooting = new TowerShooting(this, bulletFactory);

        gameObject.transform.position = position;
        _towerVisual.SetupVisual();
    }

    public Transform GetTarget()
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
