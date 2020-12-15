﻿using System.Collections;
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
        
    }

    public override void OnStateEnter()
    {
        character.animator.SetTrigger("idle");
        character.stateString = "idle";
    }
}
