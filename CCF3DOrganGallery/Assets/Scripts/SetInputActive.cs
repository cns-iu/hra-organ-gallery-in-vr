using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetInputActive : MonoBehaviour
{
    private void OnEnable()
    {
        SceneBuilder.OnSceneBuilt += () => { GetComponent<Selectable>().interactable = true; };
        HorizontalExtruder.ExtrusionUpdate += (v) => { GetComponent<Selectable>().interactable = v[0] == 0; };
    }
}
