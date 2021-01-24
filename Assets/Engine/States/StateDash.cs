using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateDash : State
{
    // Start is called before the first frame update
    public StateDash(Character character) : base(character)
    {
        
    }

    public override void OnStateEnter()
    {
        base.OnStateEnter();
        character.timer = 0;
        //Debug.Log("Dashing!");
        character.stateString = "dash";
        character.animator.SetTrigger("dash");
        if (character.inRight)
        {
            character.velocity.x = character.dashSpeed;
            character.facing = 1;
        } else
        {
            character.velocity.x = -character.dashSpeed;
            character.facing = -1;
        }
        character.blendToIdle = true;
        var duster = Instantiate(character.duster, 
                    character.pt_origin.position, Quaternion.identity);
        duster.transform.localScale = new Vector3((float)character.facing*duster.transform.localScale.x, 
                                                        duster.transform.localScale.y, 
                                                        duster.transform.localScale.z);
    }

    public override void Step()
    {
        character.velocity.x = character.dashSpeed * Mathf.Sign(character.velocity.x);
        var dir = Mathf.Sign(character.velocity.x);

        // Dash dancing
        if (dir == 1 && character.inLeft)
        {
            character.SwitchState(character.stateDash);
        } else if (dir == -1 && character.inRight)
        {
            character.SwitchState(character.stateDash);
        }


        if (character.timer >= character.dashFrames)
        {
            if (character.inRight && character.facing == 1 || character.inLeft && character.facing == -1)   // Run
            {
                character.SwitchState(character.stateRun);
                return;
            }
            else  //Stop
            {
                character.velocity.x *= 0.5f;
                character.SwitchState(character.stateIdle);
                return;
            }
        }

        if (character.inJump == 1)
        {
            character.Jump();
        }


    }

    public override void OnStateExit()
    {
        //base.OnStateExit();
        Debug.Log("Exiting!");
        character.blendToIdle = false;
    }
}
