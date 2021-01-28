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
    public InputActionAsset myInputActionAsset;
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

    private void Awake()
    {
        myInputActionAsset.Enable();
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
        var keyboard_right = false;
        var keyboard_left = false;
        var keyboard_up = false;
        var keyboard_down = false;
        var keyboard_primary = false;
        var keyboard_secondary = false;

        //Debug.Log(myInputActionAsset.FindAction("Player/Move", true).phase == InputActionPhase.Started);
        var axis = myInputActionAsset.FindAction("Player/Move", true).ReadValue<Vector2>();
        var primary = myInputActionAsset.FindAction("Player/Primary", true).ReadValue<float>();
        var jump = myInputActionAsset.FindAction("Player/Jump", true).ReadValue<float>();
        var walk = 1f-myInputActionAsset.FindAction("Player/Walk", true).ReadValue<float>();
        
        //return;
        //var gamepad = new InputControls();
        //gamepad.devices = new[] { Gamepad.all[0] };
        //Debug.Log(Gamepad.all);



        var isGamepad= Gamepad.all.Count != 0;
        Gamepad gamepad = null;
        if (isGamepad)
        {
            gamepad = Gamepad.all[0];
        }
        // var keyboard = Keyboard.current;
        //var myControls = new InputControls();
        //myControls.devices = new InputDevice[] { Keyboard.current };
        //myControls.bindingMask = InputBinding.MaskByGroup("Player");
        //myControls.Enable();
        //if (
        //InputAction primaryAction = myInputActionAsset.FindAction("PrimaryAction", true);



        //Debug.Log(InputHandler.inputs[0].inAxis[(int)InputContainer.Axis.LEFT_HORIZONTAL].val);
        if (gamepad != null)
        {
            InputHandler.inputs[0].inAxis[0].val = gamepad.leftStick.x.ReadValue();
            InputHandler.inputs[0].inAxis[1].val = gamepad.leftStick.y.ReadValue();
        } else
        {
            InputHandler.inputs[0].inAxis[0].val = axis.x * Mathf.Clamp(walk,0.3f,1f);
            InputHandler.inputs[0].inAxis[1].val = axis.y;
        }
        


        // Update Left stick
        if ( (gamepad != null && gamepad.leftStick.x.ReadValue() >= deadzone) || axis.x > 0)
        {
            InputHandler.inputs[0].Press(InputContainer.Button.IN_RIGHT);
        } else
        {
            InputHandler.inputs[0].Release(InputContainer.Button.IN_RIGHT);
        }

        if ( (gamepad != null && gamepad.leftStick.x.ReadValue() <= -deadzone) || axis.x < 0)
        {
            InputHandler.inputs[0].Press(InputContainer.Button.IN_LEFT);
        } else
        {
            InputHandler.inputs[0].Release(InputContainer.Button.IN_LEFT);
        }

        if ( (gamepad != null && gamepad.leftStick.y.ReadValue() <= -deadzone) || axis.y < 0 )
        {
            InputHandler.inputs[0].Press(InputContainer.Button.IN_DOWN);
        } else
        {
            InputHandler.inputs[0].Release(InputContainer.Button.IN_DOWN);
        }


        // Temp keyboard controls
        



        // Update Jump Button
        if ( gamepad != null && (gamepad.xButton.isPressed || gamepad.yButton.isPressed) || jump==1f)
        {
            InputHandler.inputs[0].Press(InputContainer.Button.IN_JUMP);
        } else
        {
            InputHandler.inputs[0].Release(InputContainer.Button.IN_JUMP);
        }


        // Update Attack/Primary button
        if ( (gamepad!= null && gamepad.aButton.isPressed) || primary==1f )
        {
            InputHandler.inputs[0].Press(InputContainer.Button.IN_PRIMARY);
        }
        else
        {
            InputHandler.inputs[0].Release(InputContainer.Button.IN_PRIMARY);
        }

    }
}



