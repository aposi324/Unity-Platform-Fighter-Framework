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
        //var gamepad = Gamepad.current;
        //if (gamepad == null)
        // return; // No gamepad connected
        //gamepad.leftStick.x.ReadValue() >= deadzone
        // Player One


        // P1 gets gamepad.
        /*
        var p1Controls = new MyControls();
        p1Controls.devices = new[] { Gamepad.all[0] };
        p1Controls.bindingMask = InputBinding.MaskByGroup("Gamepad");
        p1Controls.Enable();
        */

        // P2 gets keyboard&mouse.
        /*
        var p2Controls = new MyControls();
        p2Controls.devices = new[] { Keyboard.current, Mouse.current };
        p2Controls.bindingMask = InputBinding.MaskByGroup("KeyboardMouse");
        p2Controls.Enable();
        */



        //var gamepad = new InputControls();
        //gamepad.devices = new[] { Gamepad.all[0] };
        //Debug.Log(Gamepad.all);
        if (Gamepad.all.Count == 0) return;
        var gamepad = Gamepad.all[0];
        if (gamepad == null)
            return;


        //Debug.Log(InputHandler.inputs[0].inAxis[(int)InputContainer.Axis.LEFT_HORIZONTAL].val);
        InputHandler.inputs[0].inAxis[0].val = gamepad.leftStick.x.ReadValue();
        InputHandler.inputs[0].inAxis[1].val = gamepad.leftStick.y.ReadValue();



        // Update Left stick
        if (gamepad.leftStick.x.ReadValue() >= deadzone )
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

        if (gamepad.leftStick.y.ReadValue() <= -deadzone)
        {
            InputHandler.inputs[0].Press(InputContainer.Button.IN_DOWN);
        } else
        {
            InputHandler.inputs[0].Release(InputContainer.Button.IN_DOWN);
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

    }
}



