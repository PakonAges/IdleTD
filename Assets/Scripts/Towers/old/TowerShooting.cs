using UnityEngine;

public class TowerShooting
{
    private readonly Bullet.Pool _bulletPool;
    //Bullet Factory
    //My Bullet
    //My stats: reload

    //TowerCode _myTower;

    //public void Init(TowerCode tower)
    //{
    //    _myTower = tower;
    //}

    public TowerShooting(Bullet.Pool pool)
    {
        _bulletPool = pool;
    }

    public void ManageShooting()
    {
        //Wait for cd
        //Then Shoot
        //If no target/target = dead = be ready to shoot at any momment
    }

    private void Shot()
    {
        _bulletPool.Spawn();
    }

    //public void Shoot(GameObject myBullet, TowerCode myTower)
    //{
        //GameObject bulletGO = (GameObject)Instantiate(myBullet, _myTower.ShootingPoint.position, _myTower.ShootingPoint.rotation,this.transform);

        //BulletMain bullet = bulletGO.GetComponent<BulletMain>();

        //bullet.SetTower(myTower);                   //shitty solution

        //if (bullet != null)
        //{
        //    bullet.SetTarget(_myTower.MyTarget);
        //}
    //}

    //public void Lazer(LineRenderer line,  ParticleSystem effect, Light light, Transform enemy)
    //{
    //    if (!line.enabled)
    //    {
    //        line.enabled = true;
    //        effect.Play();
    //        light.enabled = true;
    //    }

    //    line.SetPosition(0, _myTower.ShootingPoint.position);
    //    line.SetPosition(1, enemy.position);

    //    Vector3 dir = _myTower.ShootingPoint.position - enemy.position;

    //    effect.transform.rotation = Quaternion.LookRotation(dir);   //rotate particle toward tower/lazet
    //    effect.transform.position = enemy.position + dir.normalized * 0.2f;         //creates offset, so particles will be emited not from inside, but with 0.5 offset
    //}
}
