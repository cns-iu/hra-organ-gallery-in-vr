using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UserInputModule : MonoBehaviour
{
    public ExplodingViewManager[] m_OrganExplodeManagers;
    public InputActionReference m_ExplodeReference;
    public float m_Step;
    public float m_ScalingFactor;
    public float m_GlobalSliderValue;

    public delegate void OnUpdateJoystickValue(float newJoystickValue);
    public static event OnUpdateJoystickValue OnUpdateJoystickValueEvent;
    public delegate void TargetedOrganUpdate(bool hasHit, RaycastHit hit);
    public static event TargetedOrganUpdate ForwardRaycastHitEvent;

    private void OnEnable()
    {
        ControllerRaycaster.RaycastEmitterEvent += ForwardRaycastHit;
    }

    private void OnDisable()
    {
        ControllerRaycaster.RaycastEmitterEvent -= ForwardRaycastHit;
    }

    void ForwardRaycastHit(bool hasHit, RaycastHit hit)
    {
        ForwardRaycastHitEvent?.Invoke(hasHit, hit);
    }

    private void Awake()
    {
        m_OrganExplodeManagers = Object.FindObjectsOfType<ExplodingViewManager>();
    }

    void Update()
    {
        m_GlobalSliderValue = ReadJoystickInput();
        RestrictAndShareJoystickValue(m_GlobalSliderValue);
    }

    private void RestrictAndShareJoystickValue(float joystickValue)
    {
        foreach (var org in m_OrganExplodeManagers)
        {
            float output = org.m_ExplodingValue;
            output += joystickValue * m_Step;

            if (output > 1)
            {
                output = 1;
            }
            if (output < 0)
            {
                output = 0;
            }

            org.m_ExplodingValue = output;
            BroadcastNewJoystickEvent(output);
        }
    }

    private float ReadJoystickInput()
    {
        return m_ExplodeReference.action.ReadValue<Vector2>().y * m_ScalingFactor;
    }

    void BroadcastNewJoystickEvent(float message)
    {
        OnUpdateJoystickValueEvent?.Invoke(message);
    }
}
