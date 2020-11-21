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

    public override void OnStateEnter()
    {
    }


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
