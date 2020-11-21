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
    public int inJump = 0;
    public bool inPrimary = false;
    public Animator animator;
    public bool canJump = true;
    public int timer;
    public int facing = 1;
    public int jumpSquatFrames = 20;
    public State currentState;
    public string stateString = "none";
    public int jumpCount = 0;

    // Moveset
    [SerializeField]
    public StateAttack attackScript;
    // Start is called before the first frame update
    void Start()
    {
        //Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 1;
        timer = 0;
        velocity = new Vector3(0f, 0f,0f);
        jumpCount = 0;
        currentState = new StateIdle(this);
    }

    void FixedUpdate()
    {
        ++timer;
        ProcessInput();
        ApplyGravity();
        if (didLand)
        {
            didLand = false;
        }
        currentState.Step();

        // Select attack
        if (inPrimary)
        {
            currentState = new AbbeJab(this);
        }

        ApplyVelocity();
        UpdateAnimator();
    }

    void ProcessInput()
    {
        inRight = Input.GetKey(KeyCode.RightArrow);
        inLeft = Input.GetKey(KeyCode.LeftArrow);
        if (Input.GetKey(KeyCode.Space))
        {
            inJump++;
        } else
        {
            inJump = 0;
        }
        Debug.Log(inJump);
        inPrimary = Input.GetKey(KeyCode.A);
    }

    public void Jump()
    {
        if (canJump && jumpCount <= 1)
        {
            jumpCount++;
            currentState = new StateJump(this);
        }
    }

    public void UpdateAnimator()
    {
        animator.SetFloat("yVelocity", velocity.y);
    }


    public void EndAttack()
    {
        currentState = new StateIdle(this);
    }

}
