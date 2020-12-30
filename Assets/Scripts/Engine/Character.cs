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
    public float damage = 0f;
    [SerializeField]
    protected int palette = 1;
    public int timerSpeed = 0;
    public bool inHitLag = false;
    // Moveset
    [SerializeField]
    public StateAttack attackScript;


    public HitBoxWrapper hitbox;
    public GameManager gameManager;

    public LayerMask playerMask;
    public ContactFilter2D playerContactFilter;

    public Collider2D[] collisionResults;

    public HitBox hitBoxData1;

    public SpriteRenderer spriteRenderer;
    private MaterialPropertyBlock _propBlock;
    /// <summary>
    /// Initialize the character
    /// </summary>
    new void Start()
    {
        jumpAlarm = ScriptableObject.CreateInstance<Alarm>();
        hitLagAlarm = ScriptableObject.CreateInstance<Alarm>();
        spriteRenderer = this.gameObject.GetComponentInChildren<SpriteRenderer>();

        timer = 0;
        velocity = new Vector3(0f, 0f,0f);
        knockback = new Vector3(0f, 0f, 0f);
        jumpCount = 0;
        currentState = new StateIdle(this);

        playerContactFilter = new ContactFilter2D();
        playerContactFilter.layerMask = playerMask;

        timerSpeed = 1;
    }

    private void Awake()
    {
        _propBlock = new MaterialPropertyBlock();

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
        UpdateRenderer();
    }


    /// <summary>
    /// Handle the input
    /// </summary>
    void ProcessInput()
    {
        inRight     = InputHandler.inputs[playerNum].isPressed  (InputContainer.Button.IN_RIGHT     );
        inLeft      = InputHandler.inputs[playerNum].isPressed  (InputContainer.Button.IN_LEFT      );
        inPrimary   = InputHandler.inputs[playerNum].isPressed  (InputContainer.Button.IN_PRIMARY   );
        inJump      = InputHandler.inputs[playerNum].timePressed(InputContainer.Button.IN_JUMP      );
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
        var hitbox = this.hitbox.boxCollider;
        if (hitbox.enabled)
        {
            collisionResults = new Collider2D[20];
            var results = hitbox.OverlapCollider(playerContactFilter, collisionResults);

            // Register hit
            for (int i = 0; i < 20; i++)
            {
                if (collisionResults[i] != null && collisionResults[i].gameObject != this.gameObject) {

                    // Prevent repeat hits
                    int victim = collisionResults[i].GetComponentsInParent<Character>()[0].playerNum;
                    if (this.hitbox.hitData.hitList.Contains(victim))
                        continue;
                    this.hitbox.hitData.hitList.Add(victim);

                    // Register the collision with the engine
                    gameManager.collisions.Add(new HitBoxCollision(collisionResults[i].gameObject, this.gameObject, this.hitbox.hitData)) ;

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
        velocity = new Vector3(0f, 0f, 0f);
        var stats = hbc.hitStats;
        damage += stats.damage;
        var kb = EntityPhysics.CalculateKnockback(30f, damage, weight, stats.baseKnockback, stats.knockBackGrowth);
        hitstunFrames = EntityPhysics.CalculateHitstun(kb);
        knockback = EntityPhysics.CalculateKnockbackComponents(kb, stats.angle);
        currentState = new StateHurt(this);
        Debug.Log("Player " + playerNum + " Damage: " + damage);

    }

    private void UpdateAlarms()
    {
        if (timerSpeed > 0)
        {
            jumpAlarm.CustomUpdate();
        }

        hitLagAlarm.CustomUpdate();
    }

    private void UpdateRenderer()
    {
        spriteRenderer.GetPropertyBlock(_propBlock);
        _propBlock.SetFloat("_PaletteNum", palette);
        spriteRenderer.SetPropertyBlock(_propBlock);
    }

    private void InstallHitBox(HitBoxData data)
    {
        hitbox.hitData = new HitBox(data);
        hitbox.hitData.angle = HelperFunctions.CorrectAngle(hitbox.hitData.angle, facing);
        Debug.Log("Hitbox angle: " + hitbox.hitData.angle + " Facing: " + facing);
    }

    private void InstallHitBox2(HitBoxData data)
    {

    }

    private void InstallHitBox3(HitBoxData data)
    {
        
    }
}
