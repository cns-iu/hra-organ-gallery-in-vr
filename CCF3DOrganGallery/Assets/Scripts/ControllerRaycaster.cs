using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerRaycaster : MonoBehaviour
{
    public delegate void ControllerRaycastHit(bool hasHit, RaycastHit hit);
    public static event ControllerRaycastHit RaycastEmitterEvent;
    void Update()
    {
        EmitRaycast();
    }

    void EmitRaycast()
    {
        RaycastHit hit;
        RaycastEmitterEvent?.Invoke(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity), hit);
        //Debug.Log(hit.collider.gameObject.name);
    }
}
