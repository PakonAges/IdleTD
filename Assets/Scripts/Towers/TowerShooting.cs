using System.Collections.Generic;
using UnityEngine;

public class TowerShooting
{
    private Tower _tower;
    private readonly Bullet.Pool _bulletPool;
    //private List<Bullet> _bullets;              //Why do I need this? here or do I need omewhat global Bullet Manager?

    private float _shootCD;

    public ShootingState TowerShootingState;

    public enum ShootingState
    {
        NoTarget,
        TargetLocked,
        TargetKilled
    }


    public TowerShooting(   Tower tower,
                            Bullet.Pool pool)
    {
        _tower = tower;
        _bulletPool = pool;
        TowerShootingState = ShootingState.NoTarget;
        _shootCD = _tower.TowerParameters.AttackDelay;
    }

    public void ManageShooting()
    {
        switch (TowerShootingState)
        {
            case ShootingState.NoTarget:
            if (_tower.GetTarget() != null)
            {
                TowerShootingState = ShootingState.TargetLocked;
            }
            else return;
            break;

            case ShootingState.TargetLocked:
            if (_shootCD <= 0)
            {
                Shoot();
            }
            else
            {
                _shootCD -= Time.deltaTime;
            }
            break;

            case ShootingState.TargetKilled://when we change state to this?
            //Whait for cd to finish
            if (_shootCD > 0)
            {
                _shootCD -= Time.deltaTime;
            }
            else
            {
                _shootCD = 0;
                TowerShootingState = ShootingState.NoTarget;
            }
            break;
        }
    }

    private void Shoot()
    {
        //_bullets.Add(_bulletPool.Spawn(_tower.TowerData.BulletData, _tower.transform.position, _tower.Target()));
        _bulletPool.Spawn(_tower.TowerData.BulletData, _tower.transform.position, _tower.GetTarget());
        _shootCD = _tower.TowerParameters.AttackDelay;
    }
}
