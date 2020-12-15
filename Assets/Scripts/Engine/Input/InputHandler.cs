using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contains an abstraction that is used to handle inputs.
/// </summary>
public class InputHandler : MonoBehaviour
{
    public static InputHandler Instance = null;
    public static InputContainer[] inputs = new InputContainer[2];

    private void Start()
    {

        if (InputHandler.Instance != null & Instance != this)
        {
            Destroy(gameObject);
        } else
        {
            Instance = this;    
        }
        for (int i = 0; i < 2; ++i)
        {
            inputs[i] = new InputContainer();
        }
    }

    public void CustomUpdate()
    {
        // Hard-coded controls for now

        // Player One
        InputHandler.inputs[0].inRight = Input.GetKey(KeyCode.RightArrow);
        InputHandler.inputs[0].inLeft  = Input.GetKey(KeyCode.LeftArrow);

        if (Input.GetKey(KeyCode.Space))
        {
            InputHandler.inputs[0].inJump++;
        }
        else
        {
            InputHandler.inputs[0].inJump = 0;
        }

        InputHandler.inputs[0].inPrimary = Input.GetKey(KeyCode.A);


    }
}



