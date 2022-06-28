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
    private CellTypeData _cellTypeData;
    // Reference to TissueBlockData script
    private TissueBlockData _tissueBlockData;
    // String for HubMap ID
    private string _hid;
    // String field to be accessed to store the data read from GitHub
    public string resultText;
    // To store the cubes that have been hit
    private List<GameObject> _hitTissueBlocks;

    private void Awake()
    {
        // Passing references of scripts on awakening
        _cellTypeData = gameObject.GetComponent<CellTypeData>();
        _tissueBlockData = gameObject.GetComponent<TissueBlockData>();
        _hid = _tissueBlockData.HubmapId;
        // Initializing the list of tissue-blocks that are hit
        _hitTissueBlocks = new List<GameObject>();
    }

    // IEnumerator for Get
    IEnumerator GetCsv()
    {
        // Fetching HubMapID from TissueBlockData script to insert in url 
        weblink = "https://raw.githubusercontent.com/hubmapconsortium/tissue-bar-graphs/static/csv/Skin_Soumya_et_al__paper/" +
                  _hid + ".csv";
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
            
            resultText = x;

            // Parsing results
            
            var data = CsvReader.Read(x);

            // Loop to log / show results
            for (var i = 0; i < data.Count ; i++)
            {
                //Code to log parsed data into the serialized fields for easy access by Visualization script
                _cellTypeData.cellType.Add((string)data[i]["\"cell_type\""]);
                _cellTypeData.count.Add((int)data[i]["\"count\""]);
                _cellTypeData.percentage.Add((float)data[i]["\"percentage\""]);
                _cellTypeData.cat.Add((string)data[i]["\"cat\""]);
                _cellTypeData.sex.Add((string)data[i]["\"sex\""]);
                _cellTypeData.exp.Add((string)data[i]["\"exp\""]);
                _cellTypeData.age.Add((int)data[i]["\"age\""]);
                var yPos = data[i]["\"y_pos\""];
                if (ReferenceEquals(yPos, ""))
                {
                    _cellTypeData.yPos.Add(0f);
                }
                else
                {
                    _cellTypeData.yPos.Add((float)yPos);
                }
            }
        }
    }

    private void OnEnable()
    {
        // Subscribes events to respective method
        TissueBlockSelectActions.OnHover += DisplayHubMapID; // Displays HubMapID when tissue-block is hovered upon
        TissueBlockSelectActions.OnSelected += LogCellTypeInfo; // Logs cell information when tissue-block is selected to CellTypeData script
    }

    private void OnDestroy()
    {
        // Unsubscribes events to respective method
        TissueBlockSelectActions.OnHover -= DisplayHubMapID;
        TissueBlockSelectActions.OnSelected -= LogCellTypeInfo;
    }

    // Responsible for printing HubMap ID when tissue-block is hovered upon
    private void DisplayHubMapID(RaycastHit hit)
    {
        if (hit.collider.gameObject.Equals(this.gameObject))
        {
            // int startHbmId = weblink.IndexOf("HBM", StringComparison.Ordinal);
            // var hubMapID = weblink.Substring(startHbmId, 15);
            Debug.Log(_hid);
        }
    }

    // Responsible for printing cell type information when tissue-block is selected
    private void LogCellTypeInfo(RaycastHit hit)
    {
        var currentHitTissueBlock = hit.collider.gameObject;
        if (!_hitTissueBlocks.Contains(currentHitTissueBlock))
        {
            if (currentHitTissueBlock.Equals(this.gameObject))
            {
                StartCoroutine(GetCsv());
            }
            _hitTissueBlocks.Add(currentHitTissueBlock);
        }
    }
    
    // Temporary fix for line break in raw Soumya et al skin data
    private string tempFormat(string csvText)
    {
        var firstIndex = csvText.IndexOf("/", StringComparison.Ordinal);
        return csvText.Remove(firstIndex - 1, 1);
    }
}
