using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpacitySetter : MonoBehaviour
{
    public GameObject m_User;
    public float m_MaxDistance = 1f;
    private Color m_ComputedColor;

    // Start is called before the first frame update
    void Start()
    {
        m_ComputedColor = GetComponent<Renderer>().material.color;
    }

    // Update is called once per frame
    void Update()
    {
        SetOpacity();
    }

    void SetOpacity()
    {
        float distance = Vector3.Distance(this.transform.position, m_User.transform.position);
        m_ComputedColor.a = Mathf.Lerp(0f, 1f, distance / m_MaxDistance);
        GetComponent<Renderer>().material.color = m_ComputedColor;
    }
}
