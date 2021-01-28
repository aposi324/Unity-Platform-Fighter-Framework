using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateFall : State
{
    private int timer;
    public StateFall(Character character) : base(character) { timer = 0; }
    public override void Step()
    {

        if (character.inJump == 1)
        {
            character.Jump();
        }

        /* Temporary Movement */
        if (character.inRight)
        {
            character.velocity.x = character.airSpeed;
        }
        else if (character.inLeft)
        {
            character.velocity.x = -character.airSpeed;
        }
        // Now handled at game entity level
        /*else
        {
            character.velocity.x = Mathf.Sign(character.velocity.x) * Mathf.Max(0f,Mathf.Abs(character.velocity.x) - character.airFriction);
            Debug.Log(character.airFriction);
        } */ 

        if (character.IsGrounded())
        {
            //character.currentState = new StateIdle(character);
            character.SwitchState(character.stateIdle);
            character.jumpCount = 0;
        }
    }

    public override void OnStateEnter()
    {
        base.OnStateEnter();
        character.canAttack = true;
        character.timer = 0;
        //character.animator.SetTrigger("fall");
    }




}
