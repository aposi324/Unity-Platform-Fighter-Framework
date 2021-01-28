using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackSlot { jab, stilt, utilt, dtilt, dashAttack, nair, fair, dair, bair, uair, ssmash, dsmash, usmash, grab, pummel };
public class StateAttack : State
{
    private AttackSlot attack;
    public StateAttack(Character character) : base(character)
    {
        character.stateString = "attack";
    }
    public override void Step()
    {
        switch (attack) {
            case AttackSlot.jab     : character.Jab()   ; break;
            case AttackSlot.nair    : character.Nair()  ; break;
            case AttackSlot.bair    : character.Bair()  ; break;
            case AttackSlot.dtilt   : character.Dtilt() ; break;
            default                 : character.Jab()   ; break;
        }
    }

    public override void OnStateExit()
    {
        base.OnStateExit();
    }

    public override void OnStateEnter()
    {
        base.OnStateEnter();
        character.timer = 0;
        attack = character.attackType;
        character.canAttack = false;
        character.stateString = "attack";
        //Debug.Log(attack);
        Step();
    }

}
