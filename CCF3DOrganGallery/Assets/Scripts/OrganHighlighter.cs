using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrganHighlighter : MonoBehaviour
{
    GameObject m_Marker;
    private void OnEnable()
    {
        UserInputModule.ForwardRaycastHitEvent += SetHighlightOnTargetUpdate;
        UserInputModule.ForwardRaycastHitEvent += EnableExplosion;
    }

    private void OnDisable()
    {
        UserInputModule.ForwardRaycastHitEvent -= SetHighlightOnTargetUpdate;
        UserInputModule.ForwardRaycastHitEvent -= EnableExplosion;
    }

    private void Awake()
    {
        m_Marker = transform.GetChild(transform.childCount - 1).gameObject;
    }

    void SetHighlightOnTargetUpdate(bool hasHit, RaycastHit hit)
    {
        m_Marker.SetActive(hasHit && hit.collider.gameObject.Equals(this.gameObject));
    }

    void EnableExplosion(bool hasHit, RaycastHit hit)
    {
        this.GetComponent<ExplodingViewManager>().enabled = hasHit && hit.collider.gameObject.Equals(this.gameObject);
    }

}
