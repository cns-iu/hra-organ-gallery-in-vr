using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExplodingViewManager : MonoBehaviour
{
    [Range(0, 1)]
    public float m_ExplodingValue;
    public List<GameObject> m_AnatomicalStructures = new List<GameObject>();
    private List<Vector3> m_StartPositions = new List<Vector3>();
    // Update is called once per frame
    private GameObject m_Organ;
    public Vector3 m_Centroid;
    private void Awake()
    {
        m_Organ = this.gameObject;
        m_AnatomicalStructures = FlattenChildrenIntoList(m_Organ);
        m_AnatomicalStructures = SimplifyLighting(m_AnatomicalStructures);
        // m_AnatomicalStructures = AddLabeler(m_AnatomicalStructures);
        m_Centroid = ComputeCentroid(m_AnatomicalStructures);
        m_StartPositions = CaptureStartPositions(m_AnatomicalStructures);
    }

    List<GameObject> SimplifyLighting(List<GameObject> list)
    {
        foreach (var item in list)
        {
            item.GetComponent<MeshRenderer>().receiveShadows = false;
            item.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        }

        return list;
    }

    private void Update()
    {
        SetPositionsByExplodingValue();
    }

    List<GameObject> AddLabeler(List<GameObject> list)
    {
        foreach (var item in list)
        {
            item.AddComponent<Labeler>();

        }
        return list;
    }

    void SetPositionsByExplodingValue()
    {
        for (int i = 0; i < m_AnatomicalStructures.Count; i++)
        {
            Vector3 destination = -Vector3.Normalize(m_Centroid - m_StartPositions[i]);
            // Debug.DrawLine(m_StartPositions[i], m_Centroid, Color.red);
            // Debug.DrawLine(m_AnatomicalStructures[i].transform.position, m_Centroid, Color.red);
            m_AnatomicalStructures[i].transform.position = Vector3.Lerp(
              m_StartPositions[i],
              destination + m_Centroid,
              m_ExplodingValue
          );
        }
    }

    private List<Vector3> CaptureStartPositions(List<GameObject> objects)
    {
        List<Vector3> result = new List<Vector3>();
        for (int i = 0; i < objects.Count; i++)
        {
            result.Add(objects[i].transform.position);
        }
        return result;
    }

    private Vector3 ComputeCentroid(List<GameObject> list)
    {
        Vector3 total = Vector3.zero;
        for (int i = 0; i < list.Count; i++)
        {
            total += list[i].transform.position;
        }
        Vector3 result = total / list.Count;
        return result;
    }

    private List<GameObject> FlattenChildrenIntoList(GameObject organ)
    {
        List<GameObject> result = new List<GameObject>();
        Utils.FindLeaves(organ.transform, result);
        return result;
    }
}
