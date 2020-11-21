using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbbeJab : StateAttack
{
    private int timer = 0;
    public AbbeJab(Character character) : base(character) { 
        timer = 0;
    }

    public override void OnStateEnter()
    {
        timer = 0;
        character.stateString = "attack";
        character.animator.SetTrigger("jab");
        character.velocity.x = 0f;
    }

    public override void Step() 
    {
        base.Step();
    }


}
