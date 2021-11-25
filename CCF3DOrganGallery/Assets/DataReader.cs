using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DataReader : MonoBehaviour
{
    public List<CellCount> m_CellCounts = new List<CellCount>();

    [SerializeField]
    private string m_Filename;
    // Start is called before the first frame update
    void Awake()
    {
        ReadCSV();

    }


    void ReadCSV()
    {
        using (var reader = new StreamReader("Assets/Data/" + m_Filename))
        {
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                string[] elements = line.Split(',');

                for (int i = 0; i < elements.Length; i++)
                {
                    elements[i] = elements[i].Replace("\"","");
                }

                if (elements[3] != "Total")
                {
                    if (elements.Length == 4)
                    {
                        m_CellCounts.Add(new CellCount(elements[1], elements[2], elements[3]));
                    }
                    else
                    {
 
                        m_CellCounts.Add(new CellCount(elements[1], elements[2] + elements[3], elements[4]));
                    }

                }

            }
        }
    }
}

public struct CellCount
{
    public string ontology_id;
    public string cellType;
    public string total;

    public CellCount(string o, string ct, string t)
    {
        this.ontology_id = o;
        this.cellType = ct;
        this.total = t;
    }


}
