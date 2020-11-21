using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateAttack : State
{
    private int timer;
    public StateAttack(Character character) : base(character)
    {
        timer = 0;
        character.stateString = "attack";
    }
    public override void Step()
    {
        ++timer;
    }

}
