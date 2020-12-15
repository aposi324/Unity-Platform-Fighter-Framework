using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct HitBox
{
    //public static int nextID;
 
    public int ID;
    public static int nextID;

    public int angle;
    public float baseKnockback;
    public float knockBackGrowth;
    public float damage;
    public float priority;
    private Dictionary<int,bool> didHit;
    public HitBox(int _angle, float _bkb, float _kbg, float _dmg, float _priority)
    {
        ID = HitBox.nextID;
        HitBox.nextID++;
        angle = _angle;
        baseKnockback = _bkb;
        knockBackGrowth = _kbg;
        damage = _dmg;
        priority = _priority;
        didHit = new Dictionary<int, bool>();
    }

    public void Refresh()
    {
        ID = HitBox.nextID;
        ++HitBox.nextID;
    }

    public string toString()
    {
        return "angle " + angle;
    }
    
}

public struct HitBoxCollision
{
    public GameObject victim;
    public GameObject attacker;
    public HitBox hitStats;
    
    public HitBoxCollision(GameObject _victim, GameObject _attacker, HitBox _hitStats)
    {
        victim = _victim;
        attacker = _attacker;
        hitStats = _hitStats;
    }

    public string toString()
    {
        return "" + hitStats.angle;
    }
}
