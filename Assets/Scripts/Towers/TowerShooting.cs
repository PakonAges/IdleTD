using UnityEngine;

public class TowerShooting
{
    private Tower _tower;
    private readonly Bullet.Pool _bulletPool;

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
            if (_tower.Target() != null)
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
        _bulletPool.Spawn(_tower.TowerData.BulletData, _tower.Target());
        _shootCD = _tower.TowerParameters.AttackDelay;
    }
}
