using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class DisableXRRay : MonoBehaviour
{
    private LineRenderer m_LineRenderer;
    private ControllerRaycaster m_ControllerRaycaster;
    private void OnEnable()
    {
        UserInputModule.CollisionWithOrganEvent += HideXRRayInteractor;
        UserInputModule.CollisionWithOrganEndEvent += ShowXRRayInteractor;
    }

    private void OnDestroy()
    {
        UserInputModule.CollisionWithOrganEvent -= HideXRRayInteractor;
        UserInputModule.CollisionWithOrganEndEvent -= ShowXRRayInteractor;
    }

    void HideXRRayInteractor(GameObject gameObject)
    {
        m_LineRenderer.enabled = false;
        m_ControllerRaycaster.enabled = false;
    }

    void ShowXRRayInteractor()
    {
        m_LineRenderer.enabled = true;
        m_ControllerRaycaster.enabled = true;
    }

    private void Awake()
    {
        m_LineRenderer = this.gameObject.GetComponent<LineRenderer>();
        m_ControllerRaycaster = this.GetComponent<ControllerRaycaster>();
    }
}
