using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HitBox", menuName = "ScriptableObjects/HitBoxData", order = 1)]
public class HitBoxData : ScriptableObject
{
    public int angle;
    public float baseKnockback;
    public float knockBackGrowth;
    public float damage;
    public float priority;
}