using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpdateText : MonoBehaviour
{
    [SerializeField] SceneBuilder sceneBuilder;
    [SerializeField] private TMP_Text text;
    [SerializeField] private string defaultText = "";

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = defaultText + "\n" + sceneBuilder.NumberOfHubmapIds.ToString();
    }
}
