using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SetPanelText : MonoBehaviour
{
    [SerializeField] private TMP_Text _cellTypeText;
    private CCFAPISPARQLQuery _query;

    private void Update()
    {
        _cellTypeText.text = $"For this tissue block, we receive:\n" +
            $"<b>{CCFAPISPARQLQuery.Instance.ExpectedCellTypes} expected cell types" +
            $"\n</b>" +
            $"The first {CCFAPISPARQLQuery.Instance.cellTypesToShare} are:\n"+
            $"{CCFAPISPARQLQuery.Instance.CellsInSelected}"
            ;
    }
    /*    void SetText(RaycastHit hit)
        {
            Debug.Log("use this for testing results" + hit.collider.gameObject.GetComponent<CCFAPISPARQLQueryTester>().Pairs;
            _cellTypeText.text = hit.collider.gameObject.GetComponent<CellTypeDataFetcher>().resultText;
        }*/
}
