using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text;
using static CCFAPISPARQLQuery;
using System.Linq;

public class SetPanelText : MonoBehaviour
{
    [SerializeField] private TMP_Text _cellTypeText;
    [SerializeField] private int _cellTypesToShow = 10;
    private Dictionary<string, int> _dictFrequencies = new Dictionary<string, int>();
    [SerializeField] private List<string> flattened = new List<string>();
    private CCFAPISPARQLQuery _query;

    private void OnEnable()
    {
        CCFAPISPARQLQuery.OnNewTissueBlockSelected += UpdateText;
    }

    private void OnDestroy()
    {
        CCFAPISPARQLQuery.OnNewTissueBlockSelected -= UpdateText;
    }


    void UpdateText(Dictionary<string, HashSet<string>> args)
    {
        GetFrequencyOfCellsPerAs(args);

        StringBuilder sb = new StringBuilder();
        sb.Append($"<b>HuBMAP ID</b>: {CCFAPISPARQLQuery.Instance.HubmapIdOfSelected}\n<b>");
        sb.Append("\n");

        sb.Append("<b>Collides with anatomical structure(s)</b>: ");
        foreach (var kvp in args)
        {
            sb.Append(kvp.Key + "\n");
        }

        sb.Append("\n");
        sb.Append($"<b>Top-{_cellTypesToShow} expected cell types</b>:\n");

        var sortedDict = from entry in _dictFrequencies orderby entry.Value descending select entry;
        var result = sortedDict.Take(_cellTypesToShow);

        foreach (var kvp in result)
        {
            sb.Append($"{kvp.Key} in {kvp.Value} AS\n");
        }
        _cellTypeText.text = sb.ToString();
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
