using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    protected Character character;

    public abstract void Step();

    public virtual void OnStateEnter() { }
    public virtual void OnStateExit() { }

    public State(Character character)
    {
        this.character = character;
        OnStateEnter();
    }
}
