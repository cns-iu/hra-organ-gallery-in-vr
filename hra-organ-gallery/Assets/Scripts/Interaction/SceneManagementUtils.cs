using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HRAOrganGallery
{
    public class SceneManagementUtils : MonoBehaviour
    {
        public static event Action<bool> OnLoadSceneStart;
        public IEnumerator LoadScene(string sceneName)
        {
            yield return null;

            //invoke event START
            OnLoadSceneStart?.Invoke(true);

            //Begin to load the Scene you specify
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
            //Don't let the Scene activate until you allow it to
            asyncOperation.allowSceneActivation = false;

            //When the load is still in progress, output the Text and progress bar
            while (!asyncOperation.isDone)
            {
                //Output the current progress
                Debug.Log("Loading progress: " + (asyncOperation.progress * 100) + "%");
                //m_Text.text = "Loading progress: " + (asyncOperation.progress * 100) + "%";

                // Check if the load has finished
                if (asyncOperation.progress >= 0.90f)
                {
                    //Activate the Scene
                    asyncOperation.allowSceneActivation = true;
                }

                yield return null;

                //invoke event END
                OnLoadSceneStart?.Invoke(false);
            }
        }
    }
}
