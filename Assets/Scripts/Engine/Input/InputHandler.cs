using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
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
        var deadzone = 0.2f;
        // Hard-coded controls for now
        var gamepad = Gamepad.current;
        if (gamepad == null)
            return; // No gamepad connected
                    //gamepad.leftStick.x.ReadValue() >= deadzone
                    // Player One


        // Update Left stick
        if (gamepad.leftStick.x.ReadValue() >= deadzone)
        {
            InputHandler.inputs[0].Press(InputContainer.Button.IN_RIGHT);
        } else
        {
            InputHandler.inputs[0].Release(InputContainer.Button.IN_RIGHT);
        }


        if (gamepad.leftStick.x.ReadValue() <= -deadzone)
        {
            InputHandler.inputs[0].Press(InputContainer.Button.IN_LEFT);
        } else
        {
            InputHandler.inputs[0].Release(InputContainer.Button.IN_LEFT);
        }


        // Update Jump Button
        if (gamepad.xButton.isPressed || gamepad.yButton.isPressed)
        {
            InputHandler.inputs[0].Press(InputContainer.Button.IN_JUMP);
        } else
        {
            InputHandler.inputs[0].Release(InputContainer.Button.IN_JUMP);
        }


        // Update Attack/Primary button
        if (gamepad.aButton.isPressed)
        {
            InputHandler.inputs[0].Press(InputContainer.Button.IN_PRIMARY);
        }
        else
        {
            InputHandler.inputs[0].Release(InputContainer.Button.IN_PRIMARY);
        }



        /*
        InputHandler.inputs[0].inLeft = gamepad.leftStick.x.ReadValue() <= -deadzone;

        InputHandler.inputs[0].inPrimary = gamepad.aButton.isPressed;
        if (gamepad.xButton.isPressed || gamepad.yButton.isPressed)
        {
            InputHandler.inputs[0].inJump++;
        } else
        {
            InputHandler.inputs[0].inJump = 0;
        }*/
    }
}



