using System;
using GameData;

[Serializable]
public class Tower {

    public int TowerId { get; set; }
    public TowerType MyTowerType { get; set; }
    public int MyTowerMapPositionX { get; set; }
    public int MyTowerMapPositionY { get; set; }
    public int MyDmg { get; set; }
    public float MyRange { get; set; }
    public float MyRateOfFire { get; set; }

    public Tower(int id,TowerType Ttype, int x, int y, int dmg, float range, float speed)
    {
        TowerId = id;
        MyTowerType = Ttype;
        MyTowerMapPositionX = x;
        MyTowerMapPositionY = y;
        MyDmg = dmg;
        MyRange = range;
        MyRateOfFire = speed;
    }

    public void UpdateParam(int dmg, float range, float speed)
    {
        MyDmg = dmg;
        MyRange = range;
        MyRateOfFire = speed;
    }
}
