using System.Collections.Generic;
using UnityEngine;

public class TowerShooting
{
    private Tower _tower;
    private readonly Bullet.Factory _bulletFactory;
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
                            Bullet.Factory factory)
    {
        _tower = tower;
        _bulletFactory = factory;
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
        _bulletFactory.Create(_tower.TowerData.BulletData, _tower.transform.position, _tower.GetTarget());
        _shootCD = _tower.TowerParameters.AttackDelay;
    }
}
