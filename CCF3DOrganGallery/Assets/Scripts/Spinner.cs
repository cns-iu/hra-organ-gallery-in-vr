using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinner : MonoBehaviour
{
    [SerializeField] private bool shouldSpin = true;
    private void OnEnable()
    {
        SceneBuilder.OnSceneBuilt += () => { shouldSpin = false; };
    }

    private void Update()
    {
        if (!shouldSpin) Destroy(this.gameObject);
        transform.Rotate(new Vector3(0, 5f, 0));
    }
}
