using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BodySystemLabeler : MonoBehaviour
{
    [SerializeField] private GameObject pre_label;
    [SerializeField] private HorizontalExtruder horizontalExtruder;

    private void OnEnable()
    {
        HorizontalExtruder.OnBodySystemsReady += AddLabels;
    }

    private void OnDestroy()
    {
        HorizontalExtruder.OnBodySystemsReady += AddLabels;
    }

    private void AddLabels(List<SystemObjectPair> list)
    {
        for (int i = 0; i < horizontalExtruder.SystemsObjs.Count; i++)
        {
            SystemObjectPair sys = horizontalExtruder.SystemsObjs[i];
            List<GameObject> organs = sys.GameObjects;
            if (organs.Count == 0) continue;

            GameObject label = Instantiate(pre_label);
            label.name = "BodySystemLabel" + sys.System.ToUpper();
            label.transform.SetParent(organs[0].transform);
            label.transform.position = new Vector3(0, 1.5f, 0);
            label.transform.GetComponentInChildren<TMP_Text>().text = sys.System;
            label.GetComponentInChildren<BackgroundOpacity>().extrudeDuringStep = 0;
            label.GetComponentInChildren<LabelOpacity>().extrudeDuringStep = 0;
            //labels.Add(label);
        }
    }
}
