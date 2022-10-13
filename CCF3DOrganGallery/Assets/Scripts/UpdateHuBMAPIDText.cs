using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpdateHuBMAPIDText : UpdateText
{
    [SerializeField] private string defaultText;
    // Start is called before the first frame update
    void Start()
    {
        GetDataSources();
        GetText();
    }

    // Update is called once per frame
    void Update()
    {
        SetText(defaultText, sceneBuilder.NumberOfHubmapIds.ToString());
    }


}
