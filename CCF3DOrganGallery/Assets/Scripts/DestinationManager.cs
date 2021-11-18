using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestinationManager : MonoBehaviour
{
    public float m_MaxDistance = 2f;
    public float m_MinDistance = 0.4f;
    public GameObject m_OrganParent;

    [SerializeField]
    private List<GameObject> m_Organs;
    private List<Destination> m_Destinations;
    private int m_NumberDestinations;
    private Vector3 m_Origin;
    // Start is called before the first frame update
    void Awake()
    {
        m_Organs = GetOrgans();
        m_NumberDestinations = m_Organs.Count;
        CreateDestinations();
    }

    void CreateDestinations()
    {
        m_Origin = this.transform.position;
        float totalLength = 2 * m_MaxDistance;
        float offset = totalLength / m_NumberDestinations;

        for (int i = 0; i < m_NumberDestinations; i++)
        {
            Destination d = new Destination(m_Origin.x + m_MaxDistance * i, m_Origin.y, m_Origin.z, m_Organs[i]);
            Debug.Log(d.position);
        }
    }

    List<GameObject> GetOrgans()
    {
        List<GameObject> organs = new List<GameObject>();
        for (int i = 0; i < m_OrganParent.transform.childCount; i++)
        {
            organs.Add(m_OrganParent.transform.GetChild(i).gameObject);
        }
        return organs;
    }
}

struct Destination
{
    public Destination(float x, float y, float z, GameObject organ)
    {
        this.organ = organ;
        this.x = x;
        this.y = y;
        this.z = z;
        this.position = new Vector3(x, y, z);
    }
    private GameObject organ;
    private float x;
    private float y;
    private float z;
    public Vector3 position;
}

struct Borders
{
    public float x1;
    public float x2;
    public float x3;
    public float x4;
}

