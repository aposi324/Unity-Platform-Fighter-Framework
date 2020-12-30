using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HelperFunctions 
{
    public static int mod2(int a, int n)
    {
        return (a - (((a / n)) * n));
    }

    public static int CorrectAngle(int _angle, int _facing)
    {
        if (_facing == 1)
        {
            return _angle;
        }
        else
        {
            _angle = HelperFunctions.mod2(_angle + 180, 360) - 180;  //get angle in range [0,360]
            return (180 - _angle);
        }
    }
}
