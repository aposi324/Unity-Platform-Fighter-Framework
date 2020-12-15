using JetBrains.Annotations;
using Microsoft.Win32.SafeHandles;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
//using UnityEditor.UI;
using UnityEngine;

public class Character : GameEntity
{
    public float jumpForce;
    public float jumpCoolDown;
    public Alarm jumpAlarm;
    public Alarm hitLagAlarm;
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
    public int playerNum = 0;

    public int timerSpeed = 0;
    public bool inHitLag = false;
    // Moveset
    [SerializeField]
    public StateAttack attackScript;


    public BoxCollider2D hitbox;
    public GameManager gameManager;

    public LayerMask playerMask;
    public ContactFilter2D playerContactFilter;

    public Collider2D[] collisionResults;

    public HitBox hitBoxData1;
    /// <summary>
    /// Initialize the character
    /// </summary>
    void Start()
    {
        jumpAlarm = ScriptableObject.CreateInstance<Alarm>();
        hitLagAlarm = ScriptableObject.CreateInstance<Alarm>();

        //Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 1;
        timer = 0;
        velocity = new Vector3(0f, 0f,0f);
        knockback = new Vector3(0f, 0f, 0f);
        jumpCount = 0;
        currentState = new StateIdle(this);
        //animator.speed = 0f;

        playerContactFilter = new ContactFilter2D();
        playerContactFilter.layerMask = playerMask;

        timerSpeed = 1;
    }

    /// <summary>
    /// Main update at fixed timestep.
    /// </summary>
    public void CustomUpdate(float deltaTime)
    {
        timer += timerSpeed;
        ProcessInput();
        UpdateAlarms();

        if (timerSpeed > 0)
        {
            currentState.Step();
            if (!inHitLag)
            {
                // Select attack
                if (inPrimary)
                {
                    currentState = new AbbeJab(this);
                }

                ApplyGravity();
                ApplyVelocity();
            }

        }
        UpdateAnimator(Time.deltaTime*timerSpeed);
    }


    /// <summary>
    /// Handle the input
    /// </summary>
    void ProcessInput()
    {
        inRight     = InputHandler.inputs[playerNum].inRight;
        inLeft      = InputHandler.inputs[playerNum].inLeft;
        inJump      = InputHandler.inputs[playerNum].inJump;
        inPrimary   = InputHandler.inputs[playerNum].inPrimary; 
    }

    /// <summary>
    /// Handle jump request. Conditions for if the jump can happen are here as well.
    /// </summary>
    public void Jump()
    {
        if (canJump && jumpCount <= 1)
        {
            jumpCount++;
            currentState = new StateJump(this);
        }
    }

    /// <summary>
    /// Update the character's animator. 
    /// </summary>
    /// <param name="t"></param>
    public void UpdateAnimator(float t)
    {
        //animator.SetFloat("time", timer * Time.deltaTime * 0.18f);
        animator.Update(t);
        animator.SetFloat("yVelocity", velocity.y);
    }

    public void EndAttack()
    {
        currentState = new StateIdle(this);
    }

    public void HandleHitCollision()
    {
        if (hitbox.enabled)
        {
            collisionResults = new Collider2D[20];
            var results = hitbox.OverlapCollider(playerContactFilter, collisionResults);

            // Register hit
            for (int i = 0; i < 20; i++)
            {
                if (collisionResults[i] != null && collisionResults[i].gameObject != this.gameObject) {
                    
                    // Register the collision with the engine
                    gameManager.collisions.Add(new HitBoxCollision(
                                                        collisionResults[i].gameObject,
                                                        this.gameObject,
                                                        new HitBox(45, 1f, 1f, 1f, 1f))
                                               );
                    // Put self in hitlag
                    // TODO: Not just timer speed
                    inHitLag = true;
                    hitLagAlarm.SetAlarm(5, () => { inHitLag = false; });
                }
            }
        }
    }

    public new void HandleHurt(HitBoxCollision hbc)
    {
        var kb = EntityPhysics.CalculateKnockback(200f, 4f, 75f, 40f, 80f);
        velocity = new Vector3(0f, 0f, 0f);
        knockback = EntityPhysics.CalculateKnockbackComponents(kb, hbc.hitStats.angle);
        hitstunFrames = EntityPhysics.CalculateHitstun(kb);
        currentState = new StateHurt(this);
    }

    private void UpdateAlarms()
    {
        if (timerSpeed > 0)
        {
            jumpAlarm.CustomUpdate();
        }

        hitLagAlarm.CustomUpdate();
    }

}
