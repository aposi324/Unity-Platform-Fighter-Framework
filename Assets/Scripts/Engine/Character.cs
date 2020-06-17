using JetBrains.Annotations;
using Microsoft.Win32.SafeHandles;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEditor.UI;
using UnityEngine;

public class Character : GameEntity
{
    public float jumpForce;
    public float jumpCoolDown;
    [SerializeField] public Alarm jumpAlarm;
    public bool inRight = false;
    public bool inLeft = false;
    public bool inJump = false;
    public Animator animator;
    public bool canJump = true;
    public int timer;
    public int facing = 1;
    public int jumpSquatFrames = 20;
    public State currentState;
    public string stateString = "none";
    // Start is called before the first frame update
    void Start()
    {
        //Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 1;
        timer = 0;
        velocity = new Vector3(0f, 0f,0f);

        currentState = new StateIdle(this);
    }

    // Update is called once per frame
    void Update()
    {
        ProcessInput();
    }

    void FixedUpdate()
    {
        ++timer;
        ApplyGravity();
        currentState.Step();
        ApplyVelocity();
        UpdateAnimator();
    }

    void ProcessInput()
    {
        inRight = Input.GetKey(KeyCode.RightArrow);
        inLeft = Input.GetKey(KeyCode.LeftArrow);
        inJump = Input.GetKey(KeyCode.Space);
    }

    public void Jump()
    {
        if (canJump)
        {
            /*
            velocity.y = jumpForce;
            canJump = false;
            jumpAlarm.SetAlarm(25, () => { canJump = true; });
            currentState = new StateJump(this);
            //((StateFall)currentState).jumping = true;
            //animator.SetTrigger("jump");
            */
            currentState = new StateJump(this);
        }
    }

    public void Foo(int bar, int bar2)
    {
        Debug.Log(bar);
    }
    public void UpdateAnimator()
    {
        animator.SetFloat("yVelocity", velocity.y);
    }
}
