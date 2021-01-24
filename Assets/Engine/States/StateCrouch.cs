using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateCrouch : State
{

    public StateCrouch(Character character) : base(character)
    {
    }

    public override void OnStateEnter()
    {
        base.OnStateEnter();
        character.stateString = "Crouch";
        character.canAttack = true;
        character.canJump = true;
        character.animator.SetTrigger("crouch");
    }

    public override void Step()
    {
        //throw new System.NotImplementedException();
        if (!character.inDown)
        {
            Debug.Log("Switcharoo");
            character.SwitchState(character.stateIdle);
        }
    }

    public override void OnStateExit()
    {
        base.OnStateExit();
    }
}
