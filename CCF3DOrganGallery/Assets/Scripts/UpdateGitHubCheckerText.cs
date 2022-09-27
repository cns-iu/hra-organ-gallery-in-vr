using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateGitHubCheckerText : UpdateText
{
    [SerializeField] private string defaultText;
    void Start()
    {
        GetDataSources();
        GetText();
    }

    // Update is called once per frame
    void Update()
    {
        SetText(defaultText, sceneBuilder.TissueBlocksWithCT.Count.ToString());
    }

}
