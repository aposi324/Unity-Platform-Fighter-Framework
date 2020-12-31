using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EntityPhysics
{
    public static float CalculateKnockback(float percent, float dmg, float weight, float bkb, float kbg)
    {
        // Smash bros formula for now
        var kb = ((((((percent + dmg * 1) / 10f + ((percent + dmg * 1) * dmg * (1 - (1 - 1) * 0.3f) / 20f)) * 
                                    1.4f * (200f / (weight + 100))) + 18f) * (kbg / 100f)) + bkb) * (1 * 1);
        kb = Mathf.Min(kb, 2500f);
        return kb;
    }

    //TODO
    public static float CalculateFixedKnockback()
    {
        return 0f;
    }

    public static int CalculateHitstun(float kb)
    {
        return (int)((kb * 0.4f) - 1);
    }

    public static int CalculateHitLag(float dmg, float multiplier)
    {
        return (int)((dmg / 3 + 3) * multiplier);
    }

    public static Vector3 CalculateKnockbackComponents(float kb, int angle)
    {
        Vector3 retVector = new Vector3(0f, 0f, 0f);
        retVector.y = kb * 0.03f * Mathf.Sin(Mathf.Deg2Rad * angle);
        retVector.x = kb * 0.03f * Mathf.Cos(Mathf.Deg2Rad * angle);
        return retVector;
    }

    public static Vector3 GetKnockbackDecay(int angle)
    {
        var _angle = angle * Mathf.Deg2Rad;
        var decay = new Vector3(0.051f * Mathf.Sin(_angle), 0.051f * Mathf.Cos(_angle), 0f);
        return decay;
    }

    public static int AngleCorrect(int facing)
    {
        return -1;
    }
}
