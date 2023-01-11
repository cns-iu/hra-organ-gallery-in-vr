using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ExtrusionDebugger : MonoBehaviour
{
  [SerializeField] private InputActionReference RightHandJoyStickAxis;

    private void OnEnable()
    {
        //HorizontalExtruder.ExtrusionUpdate += (array) => { Debug.Log($"Extrusion value 1 is {array[0]}; extrusion value 2 is {array[1]}"); };
        RightHandJoyStickAxis.action.performed += (ctx) => { Debug.Log(ctx.time); Debug.Log(ctx.startTime); };
    }
}
