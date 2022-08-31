using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BodySystemLabeler : MonoBehaviour
{
    [SerializeField] private GameObject pre_label;
    [SerializeField] private HorizontalExtruder horizontalExtruder;
    [SerializeField] private List<GameObject> labels = new List<GameObject>();

    private void OnEnable()
    {
        SceneBuilder.OnSceneBuilt += AddLabels;
    }

    private void OnDestroy()
    {
        SceneBuilder.OnSceneBuilt += AddLabels;
    }

    private void AddLabels()
    {
        //for (int i = 0; i < horizontalExtruder.; i++)
        //{
        //    GameObject organ = horizontalExtruder.Organs[i];
        //    GameObject label = Instantiate(pre_label);
        //    label.transform.SetParent(organ.transform);
        //    label.transform.position = organ.transform.GetChild(0).transform.position + new Vector3(0, 1, 0);
        //    label.transform.GetComponentInChildren<TMP_Text>().text = organ.GetComponent<OrganData>().tooltip;
        //    labels.Add(label);
        //}
    }
}
