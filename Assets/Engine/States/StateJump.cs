using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
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
        character.stateString = "Jump";
        character.canAttack = true;
        timer = 0;
        character.timer = 0;

    }

    public override void Step()
    {
        if (timer == character.jumpSquatFrames)
        {
            character.timer = 0;
            if (character.inJump > 0)
            {
                character.velocity.y = character.fullHop;
            } else
            {
                character.velocity.y = character.shortHop;
            }
            character.velocity.x *= character.groundToAirMultiplyer;

            // Rejump alarm
            character.jumpAlarm.SetAlarm(8, () => { character.canJump = true; });
        }
        if (timer > character.jumpSquatFrames)
        {
            //character.currentState = new StateFall(character);
            character.SwitchState(character.stateFall);
        }
        ++timer;
    }

    public override void OnStateExit()
    {
        base.OnStateExit();
    }
}
