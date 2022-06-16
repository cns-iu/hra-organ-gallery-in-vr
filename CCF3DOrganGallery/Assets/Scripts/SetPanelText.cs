using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SetPanelText : MonoBehaviour
{
    [SerializeField] private TMP_Text cellTypeText;

    private void OnEnable()
    {
        // TissueBlockSelectActions.OnHover += SetText;
        TissueBlockSelectActions.OnSelected += SetText;
    }

    private void OnDestroy()
    {
        // TissueBlockSelectActions.OnHover -= SetText;
        TissueBlockSelectActions.OnSelected -= SetText;
    }

    void SetText(RaycastHit hit)
    {
        cellTypeText.text = hit.collider.gameObject.GetComponent<CellTypeDataFetcher>().resultText;
    }
}
