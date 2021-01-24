using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateWalk : State
{
    public StateWalk(Character character) : base(character) 
    {
        OnStateEnter();
    }
    public override void Step()
    {
        /* Temporary Movement */
        if (character.inRight)
        {
            character.velocity.x = character.walkSpeed;
            //character.transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else if (character.inLeft)
        {
            character.velocity.x = -character.walkSpeed;
            //character.transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else
        {
            character.velocity.x = 0;
            character.SwitchState(character.stateIdle);
        }

        if (character.inJump == 1)
        {
            character.Jump();
        }
    }

    public override void OnStateEnter()
    {
        //Debug.Log("Hi mom");
        character.animator.SetTrigger("walk");
        character.stateString = "walk";
    }
}
