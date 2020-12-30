/// <summary>
/// 
/// </summary>
///
using UnityEngine;
public class InputButton 
{
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
}

public class InputAxis
{
    
}

public class InputContainer
{
    public enum Button { IN_RIGHT, IN_LEFT, IN_UP, IN_DOWN, IN_PRIMARY, IN_JUMP };
    public const int NUM_BUTTONS = 6;
    public InputButton[] inButton = new InputButton[NUM_BUTTONS];
    
    
    public InputContainer()
    {
        
        
        for (int i = 0; i < NUM_BUTTONS; ++i)
        {
            inButton[i] = new InputButton();
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