using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour
{

    public Slider m_Slider;

    void OnEnable()
    {
        LevelClicker.OnNewLevelSelected += LoadScene;
    }

    void OnDestroy()
    {
        LevelClicker.OnNewLevelSelected -= LoadScene;
    }

    void LoadScene(int sceneIndex)
    {
        m_Slider.gameObject.SetActive(true);
        StartCoroutine(LoadAsynchronously(sceneIndex)); //re-write as async function
    }

    private IEnumerator LoadAsynchronously(int sceneIndex)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneIndex);
        while (!asyncOperation.isDone)
        {
            float progress = Mathf.Clamp01(asyncOperation.progress / .9f);
            m_Slider.value = progress;
            yield return null;
        }
        m_Slider.value = 1f;
        m_Slider.gameObject.SetActive(false);
    }
}
