using UnityEngine;

public class BulletMain : MonoBehaviour {

    TowerCode _myTower;
    public GameObject impactEffect;

    float _bulletSpeed;
    //int _bulletDmg;
    float _bulletAoERadius;

    Transform _target;

    float distance;
    float height = 0.5f;

    Vector3 correction;

    public void SetTower(TowerCode tower)
    {
        _myTower = tower;
        //_bulletDmg = tower.MyDmg;
        _bulletSpeed = tower.MyBulletSpeed;
        _bulletAoERadius = tower.MyBulletAoERadius;
    }

    public void SetTarget(Transform target)
    {
        _target = target;
        distance = (_target.position - _myTower.transform.position).magnitude;
    }

    void Update () {
		if (_target == null)
        {
            gameObject.SetActive(false);
            return;
        }

        Vector3 dir = _target.position - transform.position;
        float distanceThisFrame = _bulletSpeed * Time.deltaTime;

        if (dir.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }

        correction = Vector3.up * Mathf.Sin(dir.magnitude / distance);

        transform.Translate((dir.normalized + correction.normalized * height) * distanceThisFrame, Space.World);
	}

    void HitTarget()
    {
        if (_bulletAoERadius > 0f)
        {
            AoEDamae();
        } else
        {
            Damage(_target);
        }

        gameObject.SetActive(false);
    }

    void Damage (Transform enemy)
    {
        GameObject effectInstance = (GameObject)Instantiate(impactEffect, transform.position, transform.rotation);
        Destroy(effectInstance, 2f);            //smelly code

        //if (enemy.GetComponent<CreepMain>().RecieveDamageAndDie(_bulletDmg))
        //{
        //    _myTower.MyTarget = null;
        //}
    }

    void AoEDamae()
    {
        Collider[] hitTargets = Physics.OverlapSphere(transform.position, _bulletAoERadius);

        foreach (Collider collider in hitTargets)
        {
            if (collider.tag == "Creep")                    //shitty solution
            {
                Damage(collider.transform);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _bulletAoERadius);
    }
}
