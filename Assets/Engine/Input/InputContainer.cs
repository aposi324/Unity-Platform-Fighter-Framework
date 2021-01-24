/// <summary>
/// 
/// </summary>
///
using UnityEngine;
public class InputButton 
{
    const int freshBuffer = 3;
    public bool isPressed;
    public int timePressed;

    public InputButton()
    {
        isPressed = false;
        timePressed = 0;
    }

    public void Press()
    {
        isPressed = true;
        ++timePressed;
    }

    public void Release()
    {
        isPressed = false;
        timePressed = 0;
    }

    public bool isFreshPress()
    {
        return (timePressed > 0 && timePressed <= freshBuffer);
    }
}

public class InputAxis
{
    const int freshBuffer = 3;
    public float val;
    public bool isPressed;
    public int timePressed;

    public InputAxis()
    {
        isPressed = false;
        timePressed = 0;
        val = 0f;
    }
    public void Press()
    {
        isPressed = true;
        ++timePressed;
    }

    public void Release()
    {
        isPressed = false;
        timePressed = 0;
    }

    public bool isFreshPress()
    {
        return (timePressed > 0 && timePressed <= freshBuffer);
    }
}

public class InputContainer
{
    public enum Button { IN_RIGHT, IN_LEFT, IN_UP, IN_DOWN, IN_PRIMARY, IN_JUMP };
    public enum Axis { LEFT_HORIZONTAL, LEFT_VERTICAL, RIGHT_HORIZONTAL, RIGHT_VERTICAL, LEFT_TRIGGER, RIGHT_TRIGGER };

    public const int NUM_BUTTONS = 6;
    public const int NUM_AXIS = 1*2;
    public InputButton[] inButton = new InputButton[NUM_BUTTONS];
    public InputAxis[] inAxis = new InputAxis[NUM_AXIS];
    
    public InputContainer()
    {
        for (int i = 0; i < NUM_BUTTONS; ++i)
        {
            inButton[i] = new InputButton();
        }

        for (int i = 0; i < NUM_AXIS; ++i)
        {
            inAxis[i] = new InputAxis();
        }
    }

    public void Press(Button button)
    {
        inButton[(int)button].Press();
    }


    public void Release(Button button)
    {
        inButton[(int)button].Release();
    }

    public bool isPressed(Button button)
    {
        return inButton[(int)button].isPressed;
    }



    public int timePressed(Button button)
    {
        return inButton[(int)button].timePressed;
    }
}