using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundOpacity : MonoBehaviour, IOpacitySetter
{
    private Color maxColor;
    private void OnEnable()
    {
        HorizontalExtruder.ExtrusionUpdate += SetOpacity;
    }

    private void OnDestroy()
    {
        HorizontalExtruder.ExtrusionUpdate -= SetOpacity;
    }
    public void SetOpacity(float[] stepValues)
    {
        Image i = GetComponent<Image>();
        i.color = new Color(i.color.r, i.color.g, i.color.b, Mathf.Clamp(stepValues[1], 0, maxColor.a));
    }

    private void Awake()
    {
        maxColor = GetComponent<Image>().material.color;
        SetOpacity(new float[2] { 1,0});
    }
}
