using JetBrains.Annotations;
using Microsoft.Win32.SafeHandles;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
//using UnityEditor.UI;
using UnityEngine;
public struct ControlStick
{
    public Vector2 stickVals;
    public float deadZone;
    public float Magnitude()
    {
        return Mathf.Sqrt(Mathf.Abs(stickVals.x + stickVals.y) );
    }

    public float Angle(bool isDegrees)
    {
        if (this.Magnitude() < deadZone) return 0;
        if (isDegrees) return Mathf.Rad2Deg*(Mathf.Atan2(stickVals.x, -stickVals.y) - (Mathf.PI / 2));
        return Mathf.Atan2(stickVals.x, -stickVals.y)-(Mathf.PI/2);
    }
}
public class Character : GameEntity
{
    //Stats
    [Header("Movement Stats")]
    public float shortHop;
    public float fullHop;
    public float walkSpeed;
    public float airSpeed;
    public float groundToAirMultiplyer;
    public float jumpCoolDown;

    [Header("Other stuff")]
    public Alarm jumpAlarm;
    public Alarm hitLagAlarm;
    
    public Animator animator;
    //public bool canJump = true;
    public int timer;
    public int facing = 1;
    public int jumpSquatFrames = 20;
    public State currentState;
    public string stateString = "none";
    public int jumpCount = 0;
    public int playerNum = 0;
    public float damage = 0f;
    public float dashSpeed;
    public float runSpeed;
    public int dashFrames;
    public float acceleration;
    //public float additionalAcceleration;

    public GameObject duster;

    public int hitlagTimer;

    [SerializeField]
    protected int palette = 1;
    public int timerSpeed = 0;
    public bool inHitLag = false;

    public bool canJump;
    public bool canAttack   { get; set; }
    public bool canMove;

    public bool blendToIdle;


    // Control Stuff
    public bool inRight     { get; set; }
    public bool inLeft      { get; set; }

    public bool inPrimary   { get; set; }
    public int inJump       { get; set; }
    public bool inDown      { get; set; }

    public bool isTumble;

    public ControlStick moveStick;
   

    public HitBoxWrapper hitbox;
    public GameManager gameManager;

    public LayerMask playerMask;
    public ContactFilter2D playerContactFilter;

    public Collider2D[] collisionResults;

    public HitBox hitBoxData1;

    public SpriteRenderer spriteRenderer;
    private MaterialPropertyBlock _propBlock;

    public AttackSlot attackType;

    public Vector2 position;

    // Basic States
    public StateJump    stateJump;
    public StateIdle    stateIdle;
    public StateCrouch  stateCrouch;
    public StateAttack  stateAttack;
    public StateDash    stateDash;
    public StateRun stateRun;
    public HitBoxCollision hitData;
    public StateHurt stateHurt;
    public StateFall stateFall;
    /// <summary>
    /// Initialize the character
    /// </summary>
    new public virtual void Start()
    {
        moveStick = new ControlStick();
        jumpAlarm = ScriptableObject.CreateInstance<Alarm>();
        hitLagAlarm = ScriptableObject.CreateInstance<Alarm>();
        spriteRenderer = this.gameObject.GetComponentInChildren<SpriteRenderer>();
        
        timer = 0;
        velocity = new Vector3(0f, 0f,0f);
        position = new Vector2(0f, 0f);
        knockback = new Vector3(0f, 0f, 0f);
        jumpCount = 0;

        // Pool the states
        currentState    = new StateIdle     (this);
        stateAttack     = new StateAttack   (this);
        stateCrouch     = new StateCrouch   (this);
        stateIdle       = new StateIdle     (this);
        stateDash       = new StateDash     (this);
        stateJump       = new StateJump     (this);
        stateRun        = new StateRun      (this);
        stateHurt       = new StateHurt     (this);
        stateFall        = new StateFall    (this);
        playerContactFilter.layerMask = playerMask;

        timerSpeed = 1;

        canJump = true;
        canAttack = true;

       
    }

    private void Awake()
    {
        _propBlock = new MaterialPropertyBlock();
        blendToIdle = false;
      
    }

    /// <summary>
    /// Main update at fixed timestep.
    /// </summary>
    public virtual void CustomUpdate(float deltaTime)
    {
        position = transform.position;

        ProcessInput();
        hitLagAlarm.CustomUpdate();
        hitlagTimer -= 1;
        if (hitlagTimer == 0)
        {
            inHitLag = false;
        }


        if (timerSpeed > 0)
        {
            
            if (!inHitLag)
            {
                currentState.Step();
                UpdateAlarms();
                timer += timerSpeed;
                // Select attack
                // Tilts
                if (canAttack && IsGrounded() && inPrimary && inDown)
                {
                    SwitchState(stateAttack, AttackSlot.dtilt);
                }
                else if (canAttack && IsGrounded() && inPrimary && !inLeft && !inRight && !inDown)
                {
                    SwitchState(stateAttack, AttackSlot.jab);
                }

                // Aerials
                if (!IsGrounded() && canAttack && inPrimary && (canAttack && facing == 1 && inLeft || canAttack && facing == -1 && inRight))
                {
                    //attackType = AttackSlot.bair;
                    SwitchState(stateAttack, AttackSlot.bair);
                }
                else if (canAttack && !IsGrounded() && inPrimary)
                {
                    //attackType = AttackSlot.nair;
                    SwitchState(stateAttack, AttackSlot.nair);
                }


                ApplyGravity();
                ApplyVelocity();

                UpdateAnimator();

                position = transform.position;

            }

        }

        

        UpdateRenderer();
    }


    /// <summary>
    /// Handle the input
    /// </summary>
    void ProcessInput()
    {
        inRight     = InputHandler.inputs[playerNum].isPressed  (InputContainer.Button.IN_RIGHT     );
        inLeft      = InputHandler.inputs[playerNum].isPressed  (InputContainer.Button.IN_LEFT      );
        inDown      = InputHandler.inputs[playerNum].isPressed  (InputContainer.Button.IN_DOWN      );
        inPrimary   = InputHandler.inputs[playerNum].isPressed  (InputContainer.Button.IN_PRIMARY   );
        inJump      = InputHandler.inputs[playerNum].timePressed(InputContainer.Button.IN_JUMP      );

        moveStick.stickVals = new Vector2(InputHandler.inputs[playerNum].inAxis[0].val, InputHandler.inputs[playerNum].inAxis[1].val);
       
    }

    /// <summary>
    /// Handle jump request. Conditions for if the jump can happen are here as well.
    /// </summary>
    public void Jump()
    {
        if (canJump && jumpCount <= 1)
        {
            jumpCount++;
            //currentState = new StateJump(this);
            SwitchState(stateJump);
        }
    }

    /// <summary>
    /// Update the character's animator. 
    /// </summary>
    /// <param name="t"></param>
    public void UpdateAnimator()
    {

        //animator.SetFloat("time", timer * Time.deltaTime * 0.18f);
        //animator.Update(t);
        //animator.playbackTime = timer * Time.deltaTime;

        /*
        Debug.Log((animator.GetCurrentAnimatorStateInfo(0).length));
        animator.SetFloat("time", (float)timer / (animator.GetCurrentAnimatorStateInfo(0).length/0.0166666667f));
        animator.Update(0);
        */

        animator.Update(Time.deltaTime);
        animator.SetFloat("yVelocity", velocity.y);
    }

    public void EndAttack()
    {
        if (IsGrounded())
        {
            if (inDown)
            {
                SwitchState(stateCrouch);
            } else
            {
                SwitchState(stateIdle);
            }
        } else
        {
            //currentState = new StateFall(this);
            SwitchState(stateFall);
        }
    }

    /// <summary>
    /// Registers the character's hitbox collisions with other game entities with the manager object.
    /// </summary>
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
                    //Debug.Log(collisionResults[i]);
                    
                    // Exit if we didn't hit a player
                    if (collisionResults[i].GetComponent<Character>() == null) continue;

                    // Prevent repeat hits
                    int victim = collisionResults[i].GetComponent<Character>().playerNum;
                    if (this.hitbox.hitData.hitList.Contains(victim))
                        continue;
                    this.hitbox.hitData.hitList.Add(victim);

                    // Register the collision with the engine
                    Debug.Log(collisionResults[i].gameObject.GetComponent<Character>());
                    gameManager.collisions.Add(new HitBoxCollision(collisionResults[i].GetComponent<Character>(), this, this.hitbox.hitData)) ;

                    // Put self in hitlag
                    // TODO: Not just timer speed
                    inHitLag = true;
                    hitLagAlarm.SetAlarm(30, () => { inHitLag = false; });
                }
            }
        }
    }

    /// <summary>
    /// Handles reported hitbox collisions
    /// </summary>
    /// <param name="hbc"></param>
    public new void HandleHurt(HitBoxCollision hbc)
    {
        Debug.Log("Handdleeee");
        velocity = new Vector3(0f, 0f, 0f);
        var stats = hbc.hitStats;
        damage += stats.damage;
        var kb = EntityPhysics.CalculateKnockback(damage, stats.damage, weight, stats.baseKnockback, stats.knockBackGrowth);
        hitstunFrames = EntityPhysics.CalculateHitstun(kb);
        isTumble = hitstunFrames > 32;
        knockback = EntityPhysics.CalculateKnockbackComponents(kb, stats.angle);

        
        hitData = hbc;
        //currentState = new StateHurt(this);
        SwitchState(stateHurt);
    }

    private void UpdateAlarms()
    {
        if (timerSpeed > 0)
        {
            jumpAlarm.CustomUpdate();
        }

      
    }

    private void UpdateRenderer()
    {
        spriteRenderer.GetPropertyBlock(_propBlock);
        _propBlock.SetFloat("_PaletteNum", palette);
        spriteRenderer.SetPropertyBlock(_propBlock);

        //transform.localScale.Set(Mathf.Abs(transform.localScale.x) * facing, transform.localScale.y, transform.localScale.z);
        transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * facing, transform.localScale.y, transform.localScale.z);
    }

    private void InstallHitBox(HitBoxData data)
    {
        hitbox.hitData = new HitBox(data);
        hitbox.hitData.angle = HelperFunctions.CorrectAngle(hitbox.hitData.angle, facing);
    }

    private void InstallHitBox2(HitBoxData data)
    {

    }

    private void InstallHitBox3(HitBoxData data)
    {
        
    }

    public void SwitchState(State newState)
    {
        currentState.OnStateExit();
        currentState = newState;
        newState.OnStateEnter();
    }

    public void SwitchState(State newState, AttackSlot attack)
    {
        currentState.OnStateExit();
        currentState = newState;
        attackType = attack;
        newState.OnStateEnter();
    }


    public virtual void Jab() { 
    }
    public virtual void Nair() { }
    public virtual void Dtilt() { }

    public virtual void Bair() { }

    public void OnDrawGizmos()
    {
        var size = 10f;
        Gizmos.DrawWireSphere(position, size);
        Gizmos.DrawWireSphere(position + moveStick.stickVals * size, size/2);
    }
}
