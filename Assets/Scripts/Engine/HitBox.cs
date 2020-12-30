using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HitBox : ScriptableObject
{
    //public static int nextID;

    public List<int> hitList;
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
        hitList = new List<int>();
    }

    public HitBox(HitBoxData data)
    {
        ID = HitBox.nextID;
        HitBox.nextID++;
        angle = data.angle;
        baseKnockback = data.baseKnockback;
        knockBackGrowth = data.knockBackGrowth;
        damage = data.damage;
        priority = data.priority;
        hitList = new List<int>();
    }
    public void Refresh()
    {
        ID = HitBox.nextID;
        ++HitBox.nextID;
        hitList = new List<int>();
    }

    public string toString()
    {
        return "angle " + angle;
    }
    
}

[CreateAssetMenu(fileName = "HitBox", menuName = "ScriptableObjects/HitBoxData", order = 1)]
public class HitBoxData : ScriptableObject {
    public int angle;
    public float baseKnockback;
    public float knockBackGrowth;
    public float damage;
    public float priority;
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
