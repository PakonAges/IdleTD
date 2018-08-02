using UnityEngine;
using Zenject;

public class Bullet : MonoBehaviour
{
    private float _moveSpeed;
    private int _dmg;

    public Transform Target;

    private void Update()
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

    private void HitTarget()
    {
        //do damage
        //destroy bullet
    }

    private void Reset(BulletData bulletData)
    {
        _moveSpeed = bulletData.MoveSpeed;
        _dmg = bulletData.Damage;
    }

    public class Pool : MonoMemoryPool<BulletData, Bullet>
    {
        protected override void Reinitialize(   BulletData bulletData,
                                                Bullet bullet)
        {
            bullet.Reset(bulletData);
        }
    }

    //void HitTarget()
    //{
    //    if (_bulletAoERadius > 0f)
    //    {
    //        AoEDamae();
    //    }
    //    else
    //    {
    //        Damage(_target);
    //    }

    //    gameObject.SetActive(false);
    //}

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