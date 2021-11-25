using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellAnimator : MonoBehaviour
{
    public List<GameObject> m_Cells = new List<GameObject>();

    [SerializeField]
    private GameObject m_CellDestinationsParent;
    private List<GameObject> m_Destinations = new List<GameObject>();
    private List<string> m_CellTypesFromData = new List<string>();

    private void OnEnable()
    {
        Visualizer.VisualizationBuiltEvent += OnVisualizationBuiltMoveCells;
    }

    private void OnDestroy()
    {
        Visualizer.VisualizationBuiltEvent -= OnVisualizationBuiltMoveCells;
    }

    void Start()
    {
        GetAllDestinations();
        m_Cells = Visualizer.m_Cells;
        m_CellTypesFromData = Visualizer.m_CellTypesFromData;

    }

    void GetAllDestinations()
    {
        for (int i = 0; i < m_CellDestinationsParent.transform.childCount; i++)
        {
            m_Destinations.Add(m_CellDestinationsParent.transform.GetChild(i).gameObject);
        }
    }

    void OnVisualizationBuiltMoveCells()
    {
        for (int i = 0; i < m_Cells.Count; i++)
        {

            StartCoroutine(MoveTo(m_Cells[i], m_Destinations[m_CellTypesFromData.IndexOf(m_Cells[i].GetComponent<Cell>().m_CellType)].gameObject, 2f));
        }
        ExpandCells();
    }

    void ExpandCells()
    {
        for (int i = 0; i < m_Cells.Count; i++)
        {

        }
    }

    private IEnumerator MoveTo(GameObject gameObject, GameObject destination, float time)
    {
        Vector3 startingPos = gameObject.transform.position;
        Vector3 finalPos = destination.transform.position;
        float elapsedTime = 0;

        

        while (elapsedTime < time)
        {
            float t = elapsedTime / time;
            // t = t * t * (3f - 2f * t);
            t = Mathf.Sin(t * Mathf.PI * 0.5f);
            // t = 1f - Mathf.Cos(t * Mathf.PI * 0.5f);
            gameObject.transform.position = Vector3.Lerp(startingPos, finalPos, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

}
