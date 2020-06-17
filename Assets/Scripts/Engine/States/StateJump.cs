using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateJump : State
{
    private int timer;
    // Start is called before the first frame update
    public StateJump(Character character) : base(character)
    {
        timer = 0;
        character.animator.SetTrigger("jump");
    }

    /*
          velocity.y = jumpForce;
          canJump = false;
          jumpAlarm.SetAlarm(25, () => { canJump = true; });
          currentState = new StateJump(this);
          //((StateFall)currentState).jumping = true;
          //animator.SetTrigger("jump");
          */

    public override void Step()
    {
        if (timer == character.jumpSquatFrames)
        {
            character.velocity.y = character.jumpForce;
            character.jumpAlarm.SetAlarm(8, () => { character.canJump = true; });
        }
        if (timer > character.jumpSquatFrames)
        {
            character.currentState = new StateFall(character);
        }

        
        ++timer;
        
    }
}
