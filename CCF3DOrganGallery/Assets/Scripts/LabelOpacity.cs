using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class LabelOpacity : MonoBehaviour, IOpacitySetter
{
    private void OnEnable()
    {
        HorizontalExtruder.ExtrusionUpdate += SetOpacity;
    }

    private void OnDestroy()
    {
        HorizontalExtruder.ExtrusionUpdate -= SetOpacity;
    }
    public void SetOpacity(float[] stepValues) { 
        TMP_Text t = GetComponent<TMP_Text>();
        t.color = new Color(t.color.r, t.color.g, t.color.b, stepValues[1]);
    }

    private void Awake()
    {
        SetOpacity(new float[2] { 1f,0f});
    }
}
