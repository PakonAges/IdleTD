using UnityEngine;
using Zenject;

public class Bullet : MonoBehaviour
{
    private float _moveSpeed;
    private int _dmg;

    public Transform Target;

    private void Update()
    {
        if (Target)
        {
            Vector3 dir = Target.position - transform.position;
            float distanceThisFrame = _moveSpeed * Time.deltaTime;

            if (dir.magnitude <= distanceThisFrame)
            {
                HitTarget();
                return;
            }

            transform.Translate(dir.normalized*distanceThisFrame, Space.World);
        }
    }

    private void HitTarget()
    {
        //do damage
    }

    private void Reset(BulletData bulletData, Vector3 tower, Transform target)
    {
        tower += new Vector3(0, 1, 0); //calibration form shooting point
        gameObject.transform.position = tower;
        Target = target;
        _moveSpeed = bulletData.MoveSpeed;
        _dmg = bulletData.Damage;
    }

    public class Pool : MonoMemoryPool<BulletData, Vector3, Transform, Bullet>
    {
        protected override void OnSpawned(Bullet item)
        {
            //Fill needed Data
            base.OnSpawned(item);
        }


        //Called immediately after the item is removed (used) from the pool
        protected override void Reinitialize(   BulletData bulletData,
                                                Vector3 shootingPosition,
                                                Transform target,
                                                Bullet bullet)
        {
            bullet.Reset(bulletData, shootingPosition, target);
        }
    }

    //void Damage(Transform enemy)
    //{
    //    GameObject effectInstance = (GameObject)Instantiate(impactEffect, transform.position, transform.rotation);
    //    Destroy(effectInstance, 2f);            //smelly code

    //if (enemy.GetComponent<CreepMain>().RecieveDamageAndDie(_bulletDmg))
    //{
    //    _myTower.MyTarget = null;
    //}
    //}
}