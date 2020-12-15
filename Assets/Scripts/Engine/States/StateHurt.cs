﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateHurt : State
{
    int launchAngle;
    int time;
    bool hitLagDone;
    HitBoxCollision collisionData;
    Alarm hitLagAlarm = ScriptableObject.CreateInstance<Alarm>();
    public StateHurt(Character character) : base(character)
    {
        OnStateEnter();
    }

    public StateHurt(Character character, HitBoxCollision hitBoxCollision) : base(character)
    {
        collisionData = hitBoxCollision;
        OnStateEnter();
    }

    public override void Step()
    {
        hitLagAlarm.CustomUpdate();

        character.knockback -= EntityPhysics.GetKnockbackDecay(launchAngle);
        
        if (character.timer >= time)
        {
            OnStateExit();
            character.currentState = new StateIdle(character);
        }
    }

    public override void OnStateEnter()
    {
        
        character.hitLagAlarm.SetAlarm(10, () => { character.timerSpeed = 1; });
        character.timer = 0;
        time = character.hitstunFrames;

        character.inHitLag = true;
        hitLagAlarm.SetAlarm(5, ()=> { character.inHitLag = false; });
    }

    public override void OnStateExit()
    {
        base.OnStateExit();
        character.knockback = new Vector3(0f, 0f, 0f);
        Debug.Log("Bai");
    }
}