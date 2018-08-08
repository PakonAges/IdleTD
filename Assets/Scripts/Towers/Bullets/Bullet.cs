using System;
using UnityEngine;
using Zenject;

public class Bullet : MonoBehaviour, IDisposable, IPoolable<BulletData, Vector3, Transform, IMemoryPool>
{
    IMemoryPool _pool;
    private float _moveSpeed;
    private int _dmg;

    public Transform Target;

    public void OnSpawned(BulletData bulletData, Vector3 tower, Transform target, IMemoryPool pool)
    {
        _pool = pool;
        tower += new Vector3(0, 1, 0); //calibration form shooting point
        gameObject.transform.position = tower;
        Target = target;
        _moveSpeed = bulletData.MoveSpeed;
        _dmg = bulletData.Damage;
    }

    public void Dispose()
    {
        _pool.Despawn(this);
    }

    public void OnDespawned()
    {
        _pool = null;
        _moveSpeed = 0f;
        _dmg = 0;
        Target = null;
    }

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
        var creep = Target.gameObject.GetComponent<Creep>();
        creep.TakeDamage(_dmg);
        Dispose();
    }

    public class Factory : PlaceholderFactory<BulletData, Vector3, Transform, Bullet>
    {

    }
}