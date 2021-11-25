using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellAnimator : MonoBehaviour
{
    public List<GameObject> m_Cells = new List<GameObject>();

    [SerializeField]
    private GameObject m_CellDestinationsParent;
    public List<GameObject> m_AllDestinations = new List<GameObject>();
    public GameObject m_Destination;
    private List<string> m_CellTypesFromData = new List<string>();

    private void OnEnable()
    {
        Visualizer.VisualizationBuiltEvent += OnVisualizationBuiltMoveCell;
    }

    private void OnDestroy()
    {
        Visualizer.VisualizationBuiltEvent -= OnVisualizationBuiltMoveCell;
    }

    void Awake()
    {
        m_CellDestinationsParent = GameObject.Find("CellDestinations");
        GetAllDestinations();
        m_Cells = Visualizer.m_Cells;
        m_CellTypesFromData = Visualizer.m_CellTypesFromData;
        m_Destination = m_AllDestinations[m_CellTypesFromData.IndexOf(this.GetComponent<Cell>().m_CellType)].gameObject;
        foreach (var item in m_CellTypesFromData)
        {
            Debug.Log(item);
        }
    }

    void GetAllDestinations()
    {
        for (int i = 0; i < m_CellDestinationsParent.transform.childCount; i++)
        {
            m_AllDestinations.Add(m_CellDestinationsParent.transform.GetChild(i).gameObject);
        }
    }

    void OnVisualizationBuiltMoveCell()
    {
        m_CellTypesFromData = Visualizer.m_CellTypesFromData;
        StartCoroutine(MoveTo(this.gameObject, m_Destination, 2f));
    }

    // private IEnumerator Wiggle() { 

    // }

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
