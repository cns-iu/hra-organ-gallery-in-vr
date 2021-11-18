using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputTester : MonoBehaviour
{
    public InputActionReference m_InputActionRefToTest;
    // Update is called once per frame
    void Update()
    {
        Debug.Log(m_InputActionRefToTest.action.ReadValue<Vector2>().y);
    }
}
