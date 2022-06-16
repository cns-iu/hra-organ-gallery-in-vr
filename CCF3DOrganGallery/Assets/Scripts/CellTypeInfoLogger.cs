using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using System.Text.RegularExpressions;

/* A script created to merely to log raw GitHub data to the console. It has been adapted to HubMapDataFetcher.cs, which logs the data to a series of serialized fields in a script call CellTypeData.cs[*/

public class CellTypeInfoLogger : MonoBehaviour
{
    public string weblink = "https://raw.githubusercontent.com/hubmapconsortium/tissue-bar-graphs/static/csv/Kidney_Blue_Lake/HBM537.LTFK.379.csv";
    
    // IEnumerator for Get
    IEnumerator GetCsv()
    {
        // Calls for data from raw-github-link 
        UnityWebRequest www = UnityWebRequest.Get(weblink);
        yield return www.SendWebRequest();

        // If data not received, print error
        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        // Else,read data with the CSVReader method "Read()", and print required data according to need
        else
        {
            // Show results as text
            var x = www.downloadHandler.text;
            
            List<Dictionary<string,object>> data = CsvReader.Read (x);
            
           for(var i=0; i < data.Count; i++) {
               Debug.Log("Cell Type " + data[i]["\"cell_type\""] + " " +
                         "Ontology ID " + data[i]["\"cell_type_ontology_term_id\""] + " " +
                         "Cell Count " + data[i]["\"count\""] + " " +
                         "Percentage " + data[i]["\"percentage\""]);
            }
        }
    }
    
    private void OnEnable()
    {
        // Subscribes events to respective method
        TissueBlockSelectActions.OnHover += DisplayHubMapID; // Displays HubMapID when tissue-block is hovered upon
        TissueBlockSelectActions.OnSelected += PrintCellTypeInfo; // Displays cell information when tissue-block is selected upon
    }

    private void OnDestroy()
    {
        // Unsubscribes events to respective method
        TissueBlockSelectActions.OnHover -= DisplayHubMapID;
        TissueBlockSelectActions.OnSelected -= PrintCellTypeInfo;
    }

    // Responsible for printing HubMap ID when tissue-block is hovered upon
    private void DisplayHubMapID(RaycastHit hit)
    {
        if (hit.collider.name.Equals(this.gameObject.name))
        {
            int startHbmId = weblink.IndexOf("HBM", StringComparison.Ordinal);
            var hubMapID = weblink.Substring(startHbmId, 15);
            Debug.Log(hubMapID);
        }
    }

    // Responsible for printing cell type information when tissue-block is selected
    private void PrintCellTypeInfo(RaycastHit hit)
    {
        if (hit.collider.name.Equals(this.gameObject.name))
        {
            StartCoroutine(GetCsv());
        }
    }
    
    // for later
    [Serializable]
    public class NodeArray
    {
        [SerializeField] public Node[] nodes;
    }
    
    // for later
    [Serializable]
    public class Node
    {
        public string hubMapId;
        public string cellType;
        public string ctOntologyId;
        public string cellCount;
        public string cellPercentage;
    }

    // CSVReader to read csv data. Code taken from: https://bravenewmethod.com/2014/09/13/lightweight-csv-reader-for-unity/
    private class CsvReader
    {
        static string SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
        static string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";
        static readonly char[] TrimChars = { '\"' };

        public static List<Dictionary<string, object>> Read(string file)
        {
            var list = new List<Dictionary<string, object>>();
            
            var lines = Regex.Split(file, LINE_SPLIT_RE);

            if (lines.Length <= 1) return list;

            var header = Regex.Split(lines[0], SPLIT_RE);
            for (var i = 1; i < lines.Length; i++)
            {
                var values = Regex.Split(lines[i], SPLIT_RE);
                if (values.Length == 0 || values[0] == "") continue;

                var entry = new Dictionary<string, object>();
                for (var j = 0; j < header.Length && j < values.Length; j++)
                {
                    string value = values[j];
                    value = value.TrimStart(TrimChars).TrimEnd(TrimChars).Replace("\\", "");
                    object finalValue = value;
                    if (int.TryParse(value, out var n))
                    {
                        finalValue = n;
                    }
                    else if (float.TryParse(value, out var f))
                    {
                        finalValue = f;
                    }

                    entry[header[j]] = finalValue;
                }

                list.Add(entry);
            }

            return list;
        }
    }
}