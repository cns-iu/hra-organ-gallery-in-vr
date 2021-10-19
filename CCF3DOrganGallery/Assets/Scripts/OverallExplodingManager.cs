using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverallExplodingManager : MonoBehaviour
{
    public ExplodingViewManager[] m_OrganExplodeManagers;
    public float m_Step;
    public float m_ScalingFactor;
    public float m_GlobalSliderValue;
    public delegate void OnUpdateExplodeValue(float newJoystickValue);
    public static event OnUpdateExplodeValue OnUpdateExplodeValueEvent;

    void Awake()
    {
        m_OrganExplodeManagers = Object.FindObjectsOfType<ExplodingViewManager>();
    }

    void OnEnable()
    {
        UserInputModule.OnUpdateJoystickValueEvent += OnListenForJoystickValue;
    }

    private void OnDestroy()
    {
        UserInputModule.OnUpdateJoystickValueEvent += OnListenForJoystickValue;
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnListenForJoystickValue(float newJoystickValue)
    {
        RestrictAndShareJoystickValue(newJoystickValue);
    }

    private void RestrictAndShareJoystickValue(float joystickValue)
    {
        foreach (var org in m_OrganExplodeManagers)
        {
            OrganHighlighter highlighter = org.gameObject.GetComponent<OrganHighlighter>();
            if (highlighter.m_CanBeExploded)
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
                // OnUpdateExplodeValueEvent?.Invoke(output);
            }

        }
    }
}
