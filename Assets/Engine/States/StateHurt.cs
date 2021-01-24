using System.Collections;
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
        collisionData = character.hitData;
        
        character.hitLagAlarm.SetAlarm(10, () => { character.timerSpeed = 1; });
        character.timer = 0;
        time = character.hitstunFrames;

        character.inHitLag = true;
        character.animator.SetTrigger("hurt1");


        //Debug.Log(collisionData.attacker); 
        // Face the attacker upon being hit.
        if (collisionData.attacker.position.x < character.position.x)
        {
            character.facing = -1;
        } else
        {
            character.facing = 1;
        }  

        if (character.DidLand() && character.isTumble && !character.inHitLag)
        {
            // TODO: Bounce/Tech situation
            character.velocity.x *= 0.75f;
            character.knockback.x = 0;
            character.knockback.y = 0;
        }

        hitLagAlarm.SetAlarm(5, ()=> { character.inHitLag = false; });
    }

    public override void OnStateExit()
    {
        base.OnStateExit();
        character.knockback = new Vector3(0f, 0f, 0f);

    }
}
