using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CellTypeDestinationManager : MonoBehaviour
{
    public GameObject pre_CellTypeLabel;
    public List<GameObject> m_Destinations;
    private List<string> m_CellTypesFromData = new List<string>();

    void OnEnable()
    {
        Visualizer.UniqueCellTypesDetected += PlaceDestinationsOnCircle;
    }

    void OnDestroy()
    {
        Visualizer.UniqueCellTypesDetected -= PlaceDestinationsOnCircle;
    }
    // Start is called before the first frame update
    void Awake()
    {
        m_CellTypesFromData = Visualizer.m_CellTypesFromData;
    }

    void PlaceDestinationsOnCircle()
    {
        // Debug.Log("UniqueCellTypesDetected() called");
        float radius = 0.2f;

        for (int i = 0; i < m_CellTypesFromData.Count; i++)
        {
            // Debug.Log(i);
            float offsetDegrees = i * Mathf.PI * 2f / m_CellTypesFromData.Count;
            GameObject destination = new GameObject();
            destination.name = "CellGroup" + i.ToString();
            destination.transform.position = this.transform.position;
            destination.transform.parent = this.transform;
            float x = this.transform.position.x;
            float y = Mathf.Cos(offsetDegrees) * radius + this.transform.position.y;
            float z = Mathf.Sin(offsetDegrees) * radius + this.transform.position.z;
            destination.transform.position = new Vector3(x, y, z);
            m_Destinations.Add(destination);
        }

        SetLabel();
    }

    void SetLabel()
    {
      
        for (int i = 0; i < m_Destinations.Count; i++)
        {
            GameObject label = Instantiate(pre_CellTypeLabel);
            Vector3 goal = -Vector3.Normalize(this.transform.position - m_Destinations[i].transform.position) * 0.3f;
            label.transform.position = this.transform.position + goal;
            label.transform.parent = m_Destinations[i].transform;
            // label.GetComponentInChildren<TMP_Text>().text = m_Destinations[i].transform.GetComponentInChildren<Cell>().m_CellType;
            label.GetComponentInChildren<TMP_Text>().text = m_CellTypesFromData[i];
        }
        
    }

}
