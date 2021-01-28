using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Fox : Character
{
    
    public override void Start()
    {
        base.Start();
        dashSpeed = 1.9f;
        runSpeed = 2.2f;
        dashFrames = 11;
        acceleration = 0.02f;

    }


    public override void CustomUpdate(float deltaTime)
    {
        base.CustomUpdate(deltaTime);
    }

    public override void Nair()
    {
        if (timer == 0)
        {
            animator.SetTrigger("nair");
        }
    }

    public override void Jab()
    {
        if (timer == 0)
        {
            animator.SetTrigger("jab");
        }
    }

    public override void Bair()
    {
        if (timer == 0)
        {
            animator.SetTrigger("bair");
            canAttack = false;
        }
    }
    public override void Dtilt()
    {
        if (timer == 0)
        {
            animator.SetTrigger("dtilt");
            canAttack = false;
        }
    }

}
