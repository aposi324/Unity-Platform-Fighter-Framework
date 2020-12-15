using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Character playerOne;
    public Character playerTwo;
    public InputHandler inputHandler;
    public List<HitBoxCollision> collisions;
    private void Start()
    {
        Physics.autoSimulation = false;
        collisions = new List<HitBoxCollision>();
    }
    private void FixedUpdate()
    {
        //--------------------------------
        //   Get Current inputs
        //---------------------------------
        inputHandler.CustomUpdate();


        //---------------------------------
        //   Update Each game object
        //---------------------------------
        if (playerOne != null)
            playerOne.CustomUpdate(Time.deltaTime);
        if (playerTwo != null)
            playerTwo.CustomUpdate(Time.deltaTime);


        //--------------------------------------------
        //   Simulate Unity Physics (update colliders)
        //--------------------------------------------
        Physics.Simulate(Time.deltaTime);


        //-----------------------------------
        //   Handle Collisions
        //-----------------------------------
        if (playerOne != null)
            playerOne.HandleHitCollision();
        if (playerTwo != null)
            playerTwo.HandleHitCollision();

        // For now each registered hitbox will affect the player
        // In the future, it will be the one with the highest priority
        foreach (HitBoxCollision hbc in collisions){
            hbc.victim.GetComponent<Character>().HandleHurt(hbc);
        }

        // Clear out this frame's collisions after the logic for them is done.
        collisions.Clear();

    }
}
