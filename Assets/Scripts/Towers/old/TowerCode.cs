using UnityEngine;
using GameData;
using System;

[Serializable]
public class TowerCode : MonoBehaviour {

    public Tower myTower;
    public int MyId { get; set; }

    public int MyDmg;
    public float MyRange;
    public float MyFireSpeed;

    public float MyBulletSpeed;
    public float MyBulletAoERadius;

    float _fireCountdown = 0f;

    public GameObject MyBullet;
    public Transform ShootingPoint;

    Transform _myTarget;
    public Transform MyTarget
    {
        set { _myTarget = value; }
        get { return _myTarget; }
    }

    //Lazer setup
    public ParticleSystem LazerEffect;
    public Light LazerLight;
    public bool isLazer = false;
    public LineRenderer MylazerLine;  //should use only one place for bullet/lazer ENUM 

    public TowerUpgrader MyUpgrader;
    TowerTargeting _myTargeting;
    TowerShooting _myShooting;


    void Awake()
    {
        //_myTargeting = new TowerTargeting(this);
        MyUpgrader = new TowerUpgrader(this);
        //_myShooting = gameObject.AddComponent<TowerShooting>();
        _myShooting.Init(this);

        MyUpgrader.UpdateCostOfUpgrades();
    }

    void Start()
    {
        MyBullet.GetComponent<BulletMain>().SetTower(this);
    }

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.G))
        //{
        //    if(tempTarget !=null)
        //    _myTarget = tempTarget.transform;
        //    _myShooting.SetTarget(_myTarget);
        //    Debug.Log("rock Targeted");
        //}

        if (_myTarget == null)                                      //If I dont have a target -> look for a target
        {
            //_myTarget = _myTargeting.ChooseTarget(MyRange);

            if (isLazer)
            {
                if (MylazerLine.enabled)
                {
                    MylazerLine.enabled = false;
                    LazerEffect.Stop();
                    LazerLight.enabled = false;
                }
            }

        } else                                                      //When I Have a target -> Shoot
        {
            if (isLazer)
            {
                if (IsTargetInRange(_myTarget))
                {
                    //_myShooting.Lazer(MylazerLine, LazerEffect, LazerLight, _myTarget);
                }

            } else
            {
                if (_fireCountdown <= 0 && IsTargetInRange(_myTarget))
                {
                    _myShooting.Shoot(MyBullet, this);
                    _fireCountdown = MyFireSpeed;
                }
            }
        }

        if (_fireCountdown > 0)
            _fireCountdown -= Time.deltaTime;
    }

    public bool IsTargetInRange(Transform target)
    {
        float distanceToEnemy = Vector3.Distance(transform.position, target.position);

        if (distanceToEnemy <= MyRange)
            return true;
        else
        {
            _myTarget = null;
            return false;
        }
    }

    public void SetupTowerSettings(int id, TowerType type, TowerTile tile)
    {
        MyId = id;
        //myTower = new Tower(MyId, type, tile.MyMapPositionX, tile.MyMapPositionY, MyDmg, MyRange, MyFireSpeed);
    }

    public void UpdateTowerParamsInSave()
    {
        //PlayerStats.instance.PlayerTowers.Find(twr => twr.TowerId == MyId).UpdateParam(MyDmg,MyRange,MyFireSpeed);
    } 


    //Draw Tower Range on Select
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, MyRange);
        Gizmos.color = Color.blue;
    }
}
