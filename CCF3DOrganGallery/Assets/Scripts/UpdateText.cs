using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpdateText : MonoBehaviour
{
    public SceneBuilder sceneBuilder;
    public GitHubChecker gitHubChecker;
    [SerializeField] private TMP_Text text;

    public virtual void GetDataSources()
    {
        sceneBuilder = FindObjectOfType<SceneBuilder>();
        gitHubChecker = FindObjectOfType<GitHubChecker>();
    }

    public virtual void GetText()
    {
        text = GetComponent<TMP_Text>();
    }

    public virtual void SetText(string defaulttext, string setTo)
    {
        text.text = defaulttext + "\n" + setTo;
    }
}
