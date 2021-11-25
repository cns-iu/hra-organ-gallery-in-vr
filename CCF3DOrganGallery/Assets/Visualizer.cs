using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Visualizer : MonoBehaviour
{
    public static List<GameObject> m_Cells = new List<GameObject>();
    public GameObject pre_Dot;

    public List<Color32> m_ColorHues;

    public Dictionary<string, Color> m_CTToColorMapping = new Dictionary<string, Color>();
    public DataReader m_DataReader;

    public delegate void VisualizationBuilt();
    public static event VisualizationBuilt VisualizationBuiltEvent;
    public static List<string> m_CellTypesFromData = new List<string>();

    private List<CellCount> m_CellCounts = new List<CellCount>();
    // Start is called before the first frame update

    private void Awake()
    {
        SetColorHues();
    }

    private void Start()
    {
        m_CellCounts = m_DataReader.m_CellCounts;
        DetectUniqueCellTypes();
        Visualize();
    }

    void Visualize()
    {
        for (int i = 0; i < m_CellCounts.Count; i++)
        {
            for (int j = 0; j < int.Parse(m_CellCounts[i].total); j++)
            {
                GameObject symbol = Instantiate(pre_Dot);
                symbol.GetComponent<SpriteRenderer>().color = m_ColorHues[m_CellTypesFromData.IndexOf(m_CellCounts[i].cellType)];
                symbol.AddComponent<Cell>().m_CellType = m_CellCounts[i].cellType;
                symbol.AddComponent<CellAnimator>();
                m_Cells.Add(symbol);
                
                symbol.transform.Translate(
                    Random.Range(0, .1f),
                    Random.Range(0, .1f),
                    Random.Range(0, .1f)
                );
            }
        }
        VisualizationBuiltEvent?.Invoke();
    }

    void DetectUniqueCellTypes()
    {
        for (int i = 0; i < m_CellCounts.Count; i++)
        {
            if (!m_CellTypesFromData.Contains(m_CellCounts[i].cellType))
            {
                m_CellTypesFromData.Add(m_CellCounts[i].cellType);

            }
        }
    }

    void SetColorHues()
    {
        m_ColorHues = new List<Color32>()
     {
        //  new Color(0, 0, 0),
         new Color32(233, 30, 99, 255),
         new Color32(33, 150, 243, 255),
         new Color32(255, 235, 59, 255),
         new Color32(156, 39, 176, 255),
         new Color32(76, 175, 80, 255),
         new Color32(0, 188, 212, 255),
         new Color32(255, 87, 34, 255)
     };
    }
}
