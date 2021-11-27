using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelClicker : MonoBehaviour
{
    public int m_Level;
    public delegate void LevelEvent(int sceneIndex);
    public static event LevelEvent OnNewLevelSelected;
    private void OnTriggerEnter(Collider other) {
        OnNewLevelSelected?.Invoke(m_Level);
    }
}
