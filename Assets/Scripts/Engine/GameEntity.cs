using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is the GameEntity base class. Objects that are to interact with the game's physics will inherit this class.
/// This inheritence allows certain assumptions about the properties of the other objects.
/// </summary>
public class GameEntity : MonoBehaviour
{
    public Vector3 velocity;
    public Transform pt_origin;
    public LayerMask groundMask;
    public float grav;
    public float maxFallSpeed;
    private RaycastHit2D result;    // Raycast result object to be used wherever Raycasts are necessary for collisions.
    protected bool didLand = false;
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

        CollideWithGround();    // Ground collisions, so the player lands on the ground properly.
        this.gameObject.GetComponent<Transform>().position += velocity;
    }

    /// <summary>
    /// Provides ground collision.
    /// </summary>
    public void CollideWithGround()
    {
        // Collide with ground
        if (velocity.y < 0f)
        {
            result = Physics2D.Raycast(new Vector2(pt_origin.position.x, pt_origin.position.y), new Vector2(velocity.x, velocity.y), Mathf.Abs(velocity.y), groundMask);

            if (result.collider != null)
            {
                velocity.y = -result.distance;
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
        result = Physics2D.Raycast(new Vector2(pt_origin.position.x, pt_origin.position.y), new Vector2(0f, 1f), Mathf.Abs(0.3f), groundMask);
        return result.collider != null;
    }
}