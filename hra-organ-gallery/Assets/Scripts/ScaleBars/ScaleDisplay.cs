using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HRAOrganGallery
{
    public class ScaleDisplay : MonoBehaviour
    {
        private List<TMP_Text> _texts = new List<TMP_Text>();

        private void Awake()
        {
            _texts = GetComponentsInChildren<TMP_Text>().ToList();

            Scene activeScene = SceneManager.GetActiveScene();

            // Get the name of the active scene
            string sceneName = activeScene.name;

            //Get the label from the level index
            string scale =
                ElevatorLevelManager
                    .Instance
                    .LevelList
                    .levels
                    .Where(l => l.label == sceneName)
                    .First()
                    .scale;

            //set all texts
            _texts
                .ForEach(t =>
                {
                    t.text = scale;
                });
        }
    }
}
