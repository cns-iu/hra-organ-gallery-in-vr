using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text;
using System.Linq;
using UnityEngine.UI;
using System;
using Unity.VisualScripting;
using UnityEngine.Windows;

public class SetPanelText : MonoBehaviour
{
    //[SerializeField] private TMP_Text _cellTypeText;
    [SerializeField] private List<TMP_Text> _text = new List<TMP_Text>();
    [SerializeField] private List<GameObject> _cellTypeTexts = new List<GameObject>();
    [SerializeField] private int _cellTypesToShow = 10;
    [SerializeField] private List<string> flattened = new List<string>();
    private Dictionary<string, int> _dictFrequencies = new Dictionary<string, int>();
    private CCFAPISPARQLQuery _query;

    private void OnEnable()
    {

        CCFAPISPARQLQuery.OnNewTissueBlockSelected += EnableDisplay;
        CCFAPISPARQLQuery.OnNewTissueBlockSelected += UpdateText;
    }

    private void OnDestroy()
    {
        CCFAPISPARQLQuery.OnNewTissueBlockSelected -= EnableDisplay;
        CCFAPISPARQLQuery.OnNewTissueBlockSelected -= UpdateText;
    }

    void EnableDisplay(Dictionary<string, HashSet<string>> dict)
    {
        if (!_text[2].enabled) return;

        _text[2].enabled = false;
        Image[] images = GetComponentsInChildren<Image>();
        for (int i = 0; i < images.Length; i++)
        {
            images[i].color = new Color(images[i].color.r, images[i].color.g, images[i].color.b, 1f);
        }
    }


    void UpdateText(Dictionary<string, HashSet<string>> args)
    {
        GetFrequencyOfCellsPerAs(args);

        //HuBMAP ID
        _text[0].text = CCFAPISPARQLQuery.Instance.HubmapIdOfSelected == null ? "Not from HuBMAP" : CCFAPISPARQLQuery.Instance.HubmapIdOfSelected;

        //occurs in how many AS
        StringBuilder sb = new StringBuilder();
        foreach (var kvp in args)
        {
            string input = kvp.Key.ToString();        
            string uberonId = input.Substring(input.LastIndexOf("/")+1);
            sb.Append("\u2022<indent=1em>" + uberonId + "</indent>\n");
        }
        _text[1].text = sb.ToString();

        //top 10 cells
        var sortedDict = from entry in _dictFrequencies orderby entry.Value descending select entry;
        var result = sortedDict.Take(_cellTypesToShow);

        int iterator = 0;
        foreach (var kvp in result)
        {
            TMP_Text[] texts = _cellTypeTexts[iterator].GetComponentsInChildren<TMP_Text>();
            texts[0].text = kvp.Value.ToString() + "AS";
            texts[1].text = kvp.Key.ToString().Trim('\"');
            iterator++;
        }
    }

    void GetFrequencyOfCellsPerAs(Dictionary<string, HashSet<string>> args)
    {
        _dictFrequencies.Clear();
        flattened.Clear();

        foreach (var kvp in args)
        {
            foreach (var item in kvp.Value)
            {
                flattened.Add(item);
            }
        }

        foreach (var cell in flattened)
        {
            if (_dictFrequencies.ContainsKey(cell))
            {
                _dictFrequencies[cell]++;
            }
            else
            {
                _dictFrequencies.Add(cell, 1);
            }
        }
    }

    /*    void SetText(RaycastHit hit)
        {
            Debug.Log("use this for testing results" + hit.collider.gameObject.GetComponent<CCFAPISPARQLQueryTester>().Pairs;
            _cellTypeText.text = hit.collider.gameObject.GetComponent<CellTypeDataFetcher>().resultText;
        }*/
}
