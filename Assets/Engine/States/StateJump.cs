using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateJump : State
{
    private int timer;

    // Start is called before the first frame update
    public StateJump(Character character) : base(character)
    {

    }

    public override void OnStateEnter()
    {
        character.animator.SetTrigger("jump");
        character.canAttack = true;
        timer = 0;
    }

    public override void Step()
    {
        if (timer == character.jumpSquatFrames)
        {
            if (character.inJump > 0)
            {
                character.velocity.y = character.fullHop;
            } else
            {
                character.velocity.y = character.shortHop;
            }
            character.velocity.x *= character.groundToAirMultiplyer;

            character.jumpAlarm.SetAlarm(8, () => { character.canJump = true; });
        }
        if (timer > character.jumpSquatFrames)
        {
            character.currentState = new StateFall(character);
        }
        ++timer;
    }

    public override void OnStateExit()
    {
        base.OnStateExit();
    }
}
