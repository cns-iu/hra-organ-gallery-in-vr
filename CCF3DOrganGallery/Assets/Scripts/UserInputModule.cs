using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UserInputModule : MonoBehaviour
{
    [SerializeField] private InputActionReference joystickActionReference;
    [SerializeField] private float m_ScalingFactor;
    [SerializeField] private float m_GlobalSliderValue;

    public delegate void OnUpdateJoystickValue(float newJoystickValue);
    public static event OnUpdateJoystickValue OnUpdateJoystickValueEvent;
    public delegate void TargetedOrganUpdate(bool hasHit, RaycastHit hit);
    public static event TargetedOrganUpdate ForwardRaycastHitEvent;

    public delegate void CollisionWithOrgan(GameObject gameObject);
    public static event CollisionWithOrgan CollisionWithOrganEvent;
    public delegate void CollisionWithOrganEnd();
    public static event CollisionWithOrganEnd CollisionWithOrganEndEvent;

    public static event Action<KeyCode> KeyPressed;

    private void OnEnable()
    {
        ControllerRaycaster.RaycastEmitterEvent += ForwardRaycastHit;
        CollisionEventHandler.CollisionStartEvent += ForwardCollision;
        CollisionEventHandler.CollisionEndEvent += ForwardCollisionEnd;
    }

    private void OnDisable()
    {
        ControllerRaycaster.RaycastEmitterEvent -= ForwardRaycastHit;
        CollisionEventHandler.CollisionStartEvent -= ForwardCollision;
        CollisionEventHandler.CollisionEndEvent -= ForwardCollisionEnd;
    }

    private void ForwardCollisionEnd()
    {
        CollisionWithOrganEndEvent?.Invoke();
    }

    void ForwardCollision(GameObject gameObject)
    {
        CollisionWithOrganEvent?.Invoke(gameObject);
    }

    void ForwardRaycastHit(bool hasHit, RaycastHit hit)
    {
        ForwardRaycastHitEvent?.Invoke(hasHit, hit);
    }
    void Update()
    {
        BroadcastJoystickValue(m_GlobalSliderValue);

        if (Input.GetKeyDown(KeyCode.A))
        {
            KeyPressed?.Invoke(KeyCode.A);
        }
    }

    private void BroadcastJoystickValue(float joystickValue)
    {
        m_GlobalSliderValue = ReadJoystickInput();
        OnUpdateJoystickValueEvent?.Invoke(joystickValue);
    }

    private float ReadJoystickInput()
    {
        return joystickActionReference.action.ReadValue<Vector2>().y * m_ScalingFactor;
    }

}
