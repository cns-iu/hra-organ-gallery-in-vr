using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class ExampleListener : MonoBehaviour
{
    public ButtonHandler PrimaryButtonPressHandler = null;
    public ButtonHandler SecondaryButtonPressHandler = null;
    public ButtonHandler PrimaryAxisClickHandler = null;
    public AxisHandler2D PrimaryAxisHandler = null;
    public AxisHandler triggerHandler = null;
    public KeyHandler upArrowHandler = null;
    public KeyHandler downArrowHandler = null;

    public InputActionReference RightHandJoyStickAxis;

    public void OnEnable()
    {
        PrimaryAxisClickHandler.OnButtonDown += PrintTouchButtonDown;
        PrimaryAxisClickHandler.OnButtonUp += PrintTouchButtonUp;

        PrimaryAxisHandler.OnValueChange += PrintPrimaryAxis;
        triggerHandler.OnValueChange += PrintTrigger;

        PrimaryButtonPressHandler.OnButtonDown += PrintPrimaryButtonDown;
        PrimaryButtonPressHandler.OnButtonUp += PrintPrimaryButtonUp;

        SecondaryButtonPressHandler.OnButtonDown += PrintSecondaryButtonDown;
        SecondaryButtonPressHandler.OnButtonUp += PrintSecondaryButtonUp;

        upArrowHandler.keyHeld += PrintKeyDown;
        downArrowHandler.keyHeld += PrintKeyDown;

        RightHandJoyStickAxis.action.performed += (value) => { Debug.Log($"Getting vis Input System: {value}"); };
    }

    public void OnDisable()
    {
        PrimaryAxisClickHandler.OnButtonDown -= PrintTouchButtonDown;
        PrimaryAxisClickHandler.OnButtonUp -= PrintTouchButtonUp;
        
        PrimaryAxisHandler.OnValueChange -= PrintPrimaryAxis;
        triggerHandler.OnValueChange -= PrintTrigger;

        PrimaryButtonPressHandler.OnButtonDown -= PrintPrimaryButtonDown;
        PrimaryButtonPressHandler.OnButtonUp -= PrintPrimaryButtonUp;

        SecondaryButtonPressHandler.OnButtonDown -= PrintSecondaryButtonDown;
        SecondaryButtonPressHandler.OnButtonUp -= PrintPrimaryButtonUp;

        upArrowHandler.keyHeld -= PrintKeyDown;
        downArrowHandler.keyHeld -= PrintKeyDown;
    }

    private void PrintTouchButtonDown(XRController controller)
    {
        print("touch button down");
    }

    private void PrintTouchButtonUp(XRController controller)
    {
        print("touch button up");
    }

    private void PrintPrimaryButtonDown(XRController controller)
    {
        print("primary button down");
    }

    private void PrintPrimaryButtonUp(XRController controller)
    {
        print("primary button up");
    }

    private void PrintSecondaryButtonDown(XRController controller)
    {
        print("secondary button down");
    }

    private void PrintSecondaryButtonUp(XRController controller)
    {
        print("secondary button up");
    }

    private void PrintPrimaryAxis(XRController controller, Vector2 value)
    {
        print("Joystick moved :" + value);
    }

    private void PrintTrigger(XRController controller, float value)
    {
        print("Trigger pressed: " + value);
    }

    private void PrintKeyDown(KeyCode key) {
        Debug.Log("key down: " + key);
    }
}
