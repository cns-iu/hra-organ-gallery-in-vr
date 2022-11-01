using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Siccity.GLTFUtility.GLTFAccessor.Sparse;

public class OnExtrusionSetActive : MonoBehaviour
{
    [SerializeField] private float threshold;
    private void OnEnable()
    {
        HorizontalExtruder.ExtrusionUpdate += SetActiveOnExtrusionThreshold;
    }

    private void OnDestroy()
    {
        HorizontalExtruder.ExtrusionUpdate -= SetActiveOnExtrusionThreshold;
    }

    void SetActiveOnExtrusionThreshold(float[] values)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(values[0] >= threshold);
        }
    }

    private void Awake()
    {
        SetActiveOnExtrusionThreshold(new float[2] { 0, 0 });
    }
}
