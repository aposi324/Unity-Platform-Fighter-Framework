using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateRun : State
{
    public StateRun(Character character) : base(character)
    {
        
    }

    public override void OnStateEnter()
    {
        base.OnStateEnter();
        character.timer = 0;
        character.animator.SetTrigger("run");
    }

    public override void Step()
    {
        var pleaseDirection = 0;
        if (character.inRight)
        {
            pleaseDirection = 1;
        } else if (character.inLeft)
        {
            pleaseDirection = -1;
        }


        if (character.facing != pleaseDirection)    // Turn
        {
            //character.facing = System.Math.Sign(pleaseDirection);
            character.velocity.x = character.runSpeed * pleaseDirection;
        }
        else
        {
            character.velocity.x = pleaseDirection * character.runSpeed;
        }

        // Stop
        if (!character.inRight && !character.inLeft)
        {
            character.SwitchState(character.stateIdle);
            return;
        }

        if (character.inJump == 1)
        {
            character.Jump();
        }
    }

    public override void OnStateExit()
    {
        base.OnStateExit();
    }
}
