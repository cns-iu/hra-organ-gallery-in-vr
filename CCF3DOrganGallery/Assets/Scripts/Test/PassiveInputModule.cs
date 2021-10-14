using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PassiveInputModule : MonoBehaviour
{
    public InputActionReference thumbOnJoystick;
    // Start is called before the first frame update
    void Start()
    {
        thumbOnJoystick.action.performed += delegate
        {
            Debug.Log("thumb on");
        };
    }

    // Update is called once per frame
    void Update()
    {

    }
}
