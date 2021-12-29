using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrganHighlighter : MonoBehaviour
{
    public bool m_CanBeExploded = false;
    public List<Outline> m_OutlinesList;
    private void OnEnable()
    {
        UserInputModule.ForwardRaycastHitEvent += SetHighlightOnTargetUpdate;
        UserInputModule.ForwardRaycastHitEvent += EnableExplosion;
        UserInputModule.ForwardRaycastHitEvent += DetermineIfCanBeExploded;

        UserInputModule.CollisionWithOrganEvent += SetHighlightOnTargetUpdate;
        UserInputModule.CollisionWithOrganEvent += EnableExplosion;
        UserInputModule.CollisionWithOrganEvent += DetermineIfCanBeExploded;
    }

    private void OnDisable()
    {
        UserInputModule.ForwardRaycastHitEvent -= SetHighlightOnTargetUpdate;
        UserInputModule.ForwardRaycastHitEvent -= EnableExplosion;
        UserInputModule.ForwardRaycastHitEvent -= DetermineIfCanBeExploded;

        UserInputModule.CollisionWithOrganEvent -= SetHighlightOnTargetUpdate;
        UserInputModule.CollisionWithOrganEvent -= EnableExplosion;
        UserInputModule.CollisionWithOrganEvent -= DetermineIfCanBeExploded;
    }

    private void Awake()
    {
        AddOutlineComponents();
    }

    void AddOutlineComponents()
    {
        List<GameObject> result = new List<GameObject>();
        Utils.FindLeaves(transform, result);
        foreach (GameObject go in result)
        {
            if (go.GetComponent<Outline>() == null)
            {
                go.AddComponent<Outline>();
            }
            go.GetComponent<Outline>().enabled = false;
            go.GetComponent<Outline>().OutlineWidth = 1f;
            m_OutlinesList.Add(go.GetComponent<Outline>());
        }
    }

    void DetermineIfCanBeExploded(bool hasHit, RaycastHit hit)
    {
        m_CanBeExploded = hasHit && hit.collider.gameObject.Equals(this.gameObject);
    }

    void DetermineIfCanBeExploded(GameObject gameObject)
    {
        m_CanBeExploded = (gameObject == this.gameObject);
    }

    void SetHighlightOnTargetUpdate(GameObject gameObject)
    {
        if (gameObject == this.gameObject)
        {
            foreach (var outline in m_OutlinesList)
            {
                outline.enabled = m_CanBeExploded;
            }
        }
    }

    void SetHighlightOnTargetUpdate(bool hasHit, RaycastHit hit)
    {
        // m_Marker.SetActive(m_CanBeExploded);
        foreach (var outline in m_OutlinesList)
        {
            outline.enabled = m_CanBeExploded;
        }
    }

    void EnableExplosion(bool hasHit, RaycastHit hit)
    {
        this.GetComponent<ExplodingViewManager>().enabled = m_CanBeExploded;
    }

    void EnableExplosion(GameObject gameObject)
    {
        this.GetComponent<ExplodingViewManager>().enabled = m_CanBeExploded;
    }

}
