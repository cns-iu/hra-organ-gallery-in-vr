using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CellTypeDataFetcher : MonoBehaviour
{
    // Weblink from which we will be accessing cell-type data
    public string weblink;
    // Reference to CellTypeData script
    private CellTypeData cellTypeData;
    // Reference to TissueBlockData script
    private TissueBlockData tissueBlockData;
    // String field to be accessed to store the data read from GitHub
    public string resultText;
    // To store the cubes that have been hit
    private List<GameObject> hitTissueBlocks;

    private void Awake()
    {
        // Passing references of scripts on awakening
        cellTypeData = gameObject.GetComponent<CellTypeData>();
        tissueBlockData = gameObject.GetComponent<TissueBlockData>();
        // Initializing the list of tissue-blocks that are hit
        hitTissueBlocks = new List<GameObject>();
    }
    
    // IEnumerator for Get
    IEnumerator GetCsv()
    {
        // Fetching HubMapID from TissueBlockData script to insert in url 
        weblink = "https://raw.githubusercontent.com/hubmapconsortium/tissue-bar-graphs/static/csv/Skin_Soumya_et_al__paper/" +
                  tissueBlockData.HubmapId + ".csv";
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
            var x = tempFormat(www.downloadHandler.text);

            //capturing the raw text in a variabl for debugging and demo
            resultText = x;

            // Parsing results
            var data = CsvReader.Read(x);
            for (int i = 0; i < data.Count; i++)
            {
                // key is always the header, value is the current row
                // for each key, add all its values to the corresponding list in the data component

                CellCount newCount = new CellCount()
                {
                    cellType = (string)data[i]["\"cell_type\""],
                    count = (int)data[i]["\"count\""],
                    percentage = (float)data[i]["\"percentage\""],
                    cat = (string)data[i]["\"cat\""],
                    sex = (string)data[i]["\"sex\""],
                    exp = (string)data[i]["\"exp\""],
                    age = (int)data[i]["\"age\""],
                    yPos = (float)data[0]["\"y_pos\""]
                };
                cellTypeData.cellCounts.Add(newCount);
            }


            // Loop to log / show results
            for (var i = 0; i < data.Count; i++)
            {
                //Code to log parsed data into the serialized fields for easy access by Visualization script

                // cellTypeData.CellType = data[0].Values[1];
                // cellTypeData.cellType.Add((string)data[i]["\"cell_type\""]);
                // cellTypeData.count.Add((int)data[i]["\"count\""]);
                // cellTypeData.percentage.Add((float)data[i]["\"percentage\""]);
                // cellTypeData.cat.Add((string)data[i]["\"cat\""]);
                // cellTypeData.sex.Add((string)data[i]["\"sex\""]);
                // cellTypeData.exp.Add((string)data[i]["\"exp\""]);
                // cellTypeData.age.Add((int)data[i]["\"age\""]);
                // var yPos = data[i]["\"y_pos\""];
                // if (ReferenceEquals(yPos, ""))
                // {
                //     cellTypeData.yPos.Add(0f);
                // }
                // else
                // {
                //     cellTypeData.yPos.Add((float)yPos);
                // }
            }
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
            // int startHbmId = weblink.IndexOf("HBM", StringComparison.Ordinal);
            // var hubMapID = weblink.Substring(startHbmId, 15);
            Debug.Log(tissueBlockData.HubmapId);
        }
    }

    // Responsible for printing cell type information when tissue-block is selected
    private void LogCellTypeInfo(RaycastHit hit)
    {
        var currentHitTissueBlock = hit.collider.gameObject;
        if (!hitTissueBlocks.Contains(currentHitTissueBlock))
        {
            if (currentHitTissueBlock.Equals(this.gameObject))
            {
                StartCoroutine(GetCsv());
            }
            hitTissueBlocks.Add(currentHitTissueBlock);
        }
    }

    // Temporary fix for line break in raw Soumya et al skin data
    private string tempFormat(string csvText)
    {
        var firstIndex = csvText.IndexOf("/", StringComparison.Ordinal);
        return csvText.Remove(firstIndex - 1, 1);
    }



}

