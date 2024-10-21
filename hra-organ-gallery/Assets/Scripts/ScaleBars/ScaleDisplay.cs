using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HRAOrganGallery
{
    public enum ScaleType { Global, Local}
    public class ScaleDisplay : MonoBehaviour
    {
        [SerializeField] private ScaleType scaleType;
        private List<TMP_Text> _texts = new List<TMP_Text>();

        private void Start()
        {
            _texts = GetComponentsInChildren<TMP_Text>().ToList();

            Scene activeScene = SceneManager.GetActiveScene();

            // Get the name of the active scene
            string sceneName = activeScene.name;

            //initialize variable to hold text for scale
            string scale = "SCALE";

            //Get the label from the level index
            switch (scaleType)
            {
                case ScaleType.Global:
                    scale = ElevatorLevelManager
                    .Instance
                    .LevelList
                    .levels
                    .Where(l => l.levelName == sceneName)
                    .First()
                    .scaleGlobal;

                    break;
                case ScaleType.Local:
                    scale = ElevatorLevelManager
                    .Instance
                    .LevelList
                    .levels
                    .Where(l => l.levelName == sceneName)
                    .First()
                    .scaleLocalMeasure;
                    break;
                default:
                    break;
            }
            
               

            //set all texts
            _texts
                .ForEach(t =>
                {
                    t.text = scale;
                });
        }
    }
}
