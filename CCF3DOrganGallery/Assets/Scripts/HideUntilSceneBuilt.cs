using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideUntilSceneBuilt : MonoBehaviour
{
    private void OnEnable()
    {
        SceneBuilder.OnSceneBuilt += () => gameObject.SetActive(true);
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }
}
