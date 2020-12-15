/// <summary>
/// 
/// </summary>
public class InputContainer
{
    public bool inRight;
    public bool inLeft;
    public bool inUp;
    public bool inDown;
    public bool inPrimary;
    public int inJump;
    public InputContainer()
    {
        inRight = inLeft = inUp = inDown = inPrimary = false;
        inJump = 0;
    }

}