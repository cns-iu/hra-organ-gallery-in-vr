using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetInputActive : MonoBehaviour
{
    [SerializeField] private float threshold = 0.2f;

    private void OnEnable()
    {
        SceneBuilder.OnSceneBuilt += () => { GetComponent<Selectable>().interactable = true; };
        HorizontalExtruder.ExtrusionUpdate += (v) => { GetComponent<Selectable>().interactable = v[0] <= threshold; };
    }
}
