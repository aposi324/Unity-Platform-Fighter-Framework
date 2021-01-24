using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;

public class StateIdle : State
{
    public StateIdle(Character character) : base(character) 
    {
        OnStateEnter();
        character.velocity.x = 0f;
    }

    public override void Step()
    {
        var dashTreshold = 0.6f;
        // Dash
        if (character.inRight && character.moveStick.Magnitude() > dashTreshold)
        {
            Debug.Log("What");
            character.SwitchState(character.stateDash);
            return;
        } else if (character.inLeft && character.moveStick.Magnitude() > dashTreshold)
        {
            character.SwitchState(character.stateDash);
            return;
        }


        //throw new System.NotImplementedException();
        /* Temporary Movement */
        if (character.inRight)
        {
            character.facing = 1;
            character.currentState = new StateWalk(character);
        }
        else if (character.inLeft)
        {
            character.facing = -1;
            character.currentState = new StateWalk(character);
        } 

        if (character.inJump == 1)
        {
            character.Jump();
        }
        
        if (character.inDown)
        {
            character.SwitchState(character.stateCrouch);
        }
    }

    public override void OnStateEnter()
    {
        if (!character.blendToIdle)
        {
            Debug.Log("Idle animating!");
            character.animator.SetTrigger("idle");
        }
        //character.blendToIdle = false;
        character.stateString = "idle";
        character.canAttack = true;
        character.canJump = true;
        character.jumpCount = 0;
    }
}
