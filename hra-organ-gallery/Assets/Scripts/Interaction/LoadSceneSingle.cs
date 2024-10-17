using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;

namespace HRAOrganGallery
{
    public class LoadSceneSingle : SceneManagementUtils
    {

        [SerializeField]
        private SOLevelIndex _levelIndex;

        private void Awake()
        {
            GetComponent<XRGrabInteractable>()
                .selectEntered
                .AddListener((SelectEnterEventArgs args) =>
                {
                    StartCoroutine(LoadScene(_levelIndex.levelName));
                });
        }
    }
}
