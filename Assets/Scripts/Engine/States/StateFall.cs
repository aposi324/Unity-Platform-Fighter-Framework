using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class StateFall : State
{
    private int timer;
    public bool jumping;
    public StateFall(Character character) : base(character) { timer = 0; jumping = false; }
    public override void Step()
    {
        if (timer < character.jumpSquatFrames)
        {

        }
        if (character.inJump)
        {
            character.Jump();
        }

        /* Temporary Movement */
        if (character.inRight)
        {
            character.velocity.x = 5f;
        }
        else if (character.inLeft)
        {
            character.velocity.x = -5f;
        }
        else
        {
            character.velocity.x = 0f;
        }

        if (character.IsGrounded())
        {
            character.currentState = new StateIdle(character);
        }
    }




}
