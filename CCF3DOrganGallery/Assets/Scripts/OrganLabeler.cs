using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OrganLabeler : MonoBehaviour
{
    [SerializeField] private GameObject pre_label;
    [SerializeField] private SceneBuilder sceneBuilder;
    [SerializeField] private List<GameObject> labels = new List<GameObject>();

    public virtual void OnEnable()
    {
        SceneBuilder.OnSceneBuilt += AddLabels;
    }

    public virtual void OnDestroy()
    {
        SceneBuilder.OnSceneBuilt -= AddLabels;
    }

    public void AddLabels() {
        for (int i = 0; i < sceneBuilder.Organs.Count; i++)
        {
            GameObject organ = sceneBuilder.Organs[i];
            GameObject label = Instantiate(pre_label);
            label.name = "OrganLabel" + organ.name.ToUpper();
            label.transform.SetParent(organ.transform);
            label.transform.position = organ.transform.GetChild(0).transform.position + new Vector3(0,1,0);
            label.transform.GetComponentInChildren<TMP_Text>().text = organ.GetComponent<OrganData>().tooltip;
            labels.Add(label);
        }
    }
}
