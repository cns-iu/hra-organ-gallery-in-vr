using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestListener : MonoBehaviour
{
  private void OnEnable() {
        SceneBuilder.OnSceneBuilt += () => Debug.Log("scene built!");
    }


}
