using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[CreateAssetMenu(fileName = "NewKeyHandler")]
public class KeyHandler : InputHandler
{
    public KeyCode keyCode;
    public event Action<KeyCode> keyHeld;

    public override void HandleState(XRController controller)
    {
        if (Input.GetKey(keyCode))
        {
            keyHeld?.Invoke(keyCode);
        }
    }
}
