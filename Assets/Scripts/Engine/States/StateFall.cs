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
            character.jumpCount = 0;
        }
    }




}
