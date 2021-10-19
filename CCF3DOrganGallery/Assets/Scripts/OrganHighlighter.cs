using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrganHighlighter : MonoBehaviour
{
    public GameObject pre_Marker;
    public bool m_CanBeExploded = false;
    public GameObject m_Marker;
    private void OnEnable()
    {
        UserInputModule.ForwardRaycastHitEvent += SetHighlightOnTargetUpdate;
        UserInputModule.ForwardRaycastHitEvent += EnableExplosion;
        UserInputModule.ForwardRaycastHitEvent += DetermineIfCanBeExploded;
    }

    private void OnDisable()
    {
        UserInputModule.ForwardRaycastHitEvent -= SetHighlightOnTargetUpdate;
        UserInputModule.ForwardRaycastHitEvent -= EnableExplosion;
        UserInputModule.ForwardRaycastHitEvent -= DetermineIfCanBeExploded;
    }

    private void Awake()
    {
        m_Marker = Instantiate(pre_Marker, transform.position, Quaternion.identity);
        m_Marker.SetActive(false);
    }

    void CreateAndGetMarker()
    {

        // m_Marker = transform.GetChild(transform.childCount - 1).gameObject;
    }

    void DetermineIfCanBeExploded(bool hasHit, RaycastHit hit)
    {
        m_CanBeExploded = hasHit && hit.collider.gameObject.Equals(this.gameObject);
    }

    void SetHighlightOnTargetUpdate(bool hasHit, RaycastHit hit)
    {
        m_Marker.SetActive(m_CanBeExploded);
    }

    void EnableExplosion(bool hasHit, RaycastHit hit)
    {
        this.GetComponent<ExplodingViewManager>().enabled = m_CanBeExploded;
    }

}
