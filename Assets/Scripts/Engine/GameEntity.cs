using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is the GameEntity base class. Objects that are to interact with the game's physics will inherit this class.
/// This inheritence allows certain assumptions about the properties of the other objects.
/// </summary>
public class GameEntity : MonoBehaviour
{
    public static int nextID;
    public int ID;

    public Vector3 velocity;
    public Vector3 knockback;
    public Vector3 compositeVelocity;  // All forces merged, recalculated every frame

    public Transform pt_origin;
    public LayerMask groundMask;
    public float grav;
    public float maxFallSpeed;
    private RaycastHit2D result;    // Raycast result object to be used wherever Raycasts are necessary for collisions.
    protected bool didLand = false;
    public bool placeLock = false;

    public int launchAngle;
    public int hitstunFrames;
    public void Start()
    {
        ID = nextID;
        GameEntity.nextID++;
    }

    /// <summary>
    /// Apply the gravity force to the velocity vector.
    /// </summary>
    public void ApplyGravity()
    {
        velocity.y = Mathf.Max(maxFallSpeed, velocity.y + grav);
    }

    /// <summary>
    /// Apply the velocity vector to the GameEntity's position. Performc
    /// </summary>
    public void ApplyVelocity()
    {
        compositeVelocity = velocity + knockback;
        CollideWithGround();    // Ground collisions, so the player lands on the ground properly.
        this.gameObject.GetComponent<Transform>().position += compositeVelocity;

    }



    /// <summary>
    /// Provides ground collision.
    /// </summary>
    public void CollideWithGround()
    {
        // Collide with ground
        if (compositeVelocity.y < 0f)
        {
            result = Physics2D.Raycast(new Vector2(pt_origin.position.x, pt_origin.position.y), new Vector2(compositeVelocity.x, compositeVelocity.y), Mathf.Abs(compositeVelocity.y), groundMask);

            if (result.collider != null)
            {
                compositeVelocity.y = -result.distance;
                didLand = true;
            }
        }
    }  

    /// <summary>
    /// Returns true if the game entity is touching the ground.
    /// </summary>
    /// <returns> True if the entity is touching the ground.</returns>
    public bool IsGrounded()
    {
        result = Physics2D.Raycast(new Vector2(pt_origin.position.x, pt_origin.position.y), new Vector2(0f, 0.01f), Mathf.Abs(0.3f), groundMask);
        return result.collider != null;
    }

    public void HandleHurt(HitBoxCollision hbc)
    {
        Debug.Log(hbc.toString());
        var kb = 30f;
        //TODO: Calculate KB
        knockback.x = kb * Mathf.Sin(Mathf.Deg2Rad * hbc.hitStats.angle);
        knockback.y = kb * Mathf.Cos(Mathf.Deg2Rad * hbc.hitStats.angle);
    }




}