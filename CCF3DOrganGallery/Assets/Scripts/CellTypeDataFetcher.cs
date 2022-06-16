using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;

public class CellTypeDataFetcher : MonoBehaviour
{
    // Weblink from which we will be accessing cell-type data
    public string weblink;
    // Reference to CellTypeData script
    private CellTypeData _cellTypeData;
    // Reference to TissueBlockData script
    private TissueBlockData _tissueBlockData;
    // String field to be accessed to store the data read from GitHub
    public string resultText;

    private void Awake()
    {
        // Passing references of scripts on awakening
        _cellTypeData = gameObject.GetComponent<CellTypeData>();
        _tissueBlockData = gameObject.GetComponent<TissueBlockData>();
    }

    // IEnumerator for Get
    IEnumerator GetCsv()
    {
        // Fetching HubMapID from TissueBlockData script to insert in url 
        string hid = _tissueBlockData.HubmapId;
        weblink = "https://raw.githubusercontent.com/hubmapconsortium/tissue-bar-graphs/static/csv/Skin_Soumya_et_al__paper/" +
                  hid + ".csv";
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
            resultText = x;
            // Parsing results
            var data = CsvReader.Read(x);

            // Loop to log / show results
            // for (var i = 0; i < data.Count - 1; i++)
            // {
            //     //Test (will remove once the dataset and code is fixed)
            //     Debug.Log(data[i]["\"cell_type\""]);
            //     // Debug.Log(data[i]["\"cat\""]);
            //     // Debug.Log(data[i]["\"sex\""]);
            //     // Debug.Log(data[i]["\"age\""]);

            //     //Code to log parsed data into the serialized fields for easy access by Visualization script
            //     _cellTypeData.cell_type[i] = (string)data[i]["\"cell_type\""];
            //     // _cellTypeData.count[i] =  (int)data[i]["count"];
            //     // _cellTypeData.percentage[i] = (float)data[i]["\"percentage\""];
            //     // _cellTypeData.cat[i] = (string)data[i]["\"cat\""];
            //     // _cellTypeData.sex[i] = (string)data[i]["\"sex\""];
            //     // _cellTypeData.exp[i] = (string)data[i]["\"exp\""];
            //     // _cellTypeData.age[i] = (int)data[i]["\"age\""];
            //     // _cellTypeData.y_pos[i] = (float)data[i]["\"y_pos\""];

            //     // Supplementary string variable holding all the data to show proof of concept temporarily.
            //     resultText = "Cell Type " + data[i]["\"cell_type\""] + " " +
            //                  "Cell Count " + data[i]["\"count\""] + " " +
            //                  "Percentage " + data[i]["\"percentage\""] + " " +
            //                  "Cat " + data[i]["\"cat\""] + " " +
            //                  "Sex " + data[i]["\"sex\""] + " " +
            //                  "Exp " + data[i]["\"exp\""] + " " +
            //                  "Age " + data[i]["\"age\""] + " " +
            //                  "Y_Po " + data[i]["\"y_pos\""] + " ";
            // }
        }
    }

    private void OnEnable()
    {
        // Subscribes events to respective method
        // TissueBlockSelectActions.OnHover += DisplayHubMapID; // Displays HubMapID when tissue-block is hovered upon
        TissueBlockSelectActions.OnSelected += LogCellTypeInfo; // Logs cell information when tissue-block is selected to CellTypeData script
    }

    private void OnDestroy()
    {
        // Unsubscribes events to respective method
        // TissueBlockSelectActions.OnHover -= DisplayHubMapID;
        TissueBlockSelectActions.OnSelected -= LogCellTypeInfo;
    }

    // Responsible for printing HubMap ID when tissue-block is hovered upon
    private void DisplayHubMapID(RaycastHit hit)
    {
        if (hit.collider.gameObject.Equals(this.gameObject))
        {
            int startHbmId = weblink.IndexOf("HBM", StringComparison.Ordinal);
            var hubMapID = weblink.Substring(startHbmId, 15);
            Debug.Log(hubMapID);
        }
    }

    // Responsible for printing cell type information when tissue-block is selected
    private void LogCellTypeInfo(RaycastHit hit)
    {
        if (hit.collider.gameObject.Equals(this.gameObject))
        {
            StartCoroutine(GetCsv());
        }
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
