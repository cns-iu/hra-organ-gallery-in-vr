using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using Packages.Rider.Editor.UnitTesting;

public class SetPanelText : MonoBehaviour
{
    [SerializeField] private TMP_Text _cellTypeText;
    [SerializeField] private CCFAPISPARQLQueryTester _tester;

    private void OnEnable()
    {
        //TissueBlockSelectActions.OnHover += SetText;
        //TissueBlockSelectActions.OnSelected += SetText;
    }

    private void OnDestroy()
    {
        //TissueBlockSelectActions.OnHover -= SetText;
        //TissueBlockSelectActions.OnSelected -= SetText;
    }
    private void Awake()
    {
        _tester = FindObjectOfType<CCFAPISPARQLQueryTester>();


    }

    private void Update()
    {
        _cellTypeText.text = $"For this tissue block, we receive: { _tester.Pairs}" ;
    }
/*    void SetText(RaycastHit hit)
    {
        Debug.Log("use this for testing results" + hit.collider.gameObject.GetComponent<CCFAPISPARQLQueryTester>().Pairs;
        _cellTypeText.text = hit.collider.gameObject.GetComponent<CellTypeDataFetcher>().resultText;
    }*/
}
