using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Alarm : ScriptableObject
{
    private Action alarmCallback;
    private int timer;
    private bool _isSet = false;

    public void SetAlarm(int timer, Action alarmCallback, bool isPrecise = true)
    {
        this._isSet = true;
        this.timer = timer;
        this.alarmCallback = alarmCallback;
    }

    public void CustomUpdate()
    {
        if (timer > 0)
        {
            timer -= 1;

            if (isAlarmComplete())
            {
                _isSet = false;
                alarmCallback();
            }
        }
    }

    public bool isAlarmComplete()
    {
        return timer <= 0f && _isSet;
    }
}
