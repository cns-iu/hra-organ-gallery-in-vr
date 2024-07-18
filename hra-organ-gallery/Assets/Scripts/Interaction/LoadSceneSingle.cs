using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;

namespace HRAOrganGallery
{
    public class LoadSceneSingle : SceneManagementUtils
    {
        private XRGrabInteractable _interactable;
        [SerializeField] private string _sceneToLoad;

        private void Awake()
        {
            GetComponent<XRGrabInteractable>().selectEntered.AddListener(
            (SelectEnterEventArgs args) =>
            {
                StartCoroutine(LoadScene(_sceneToLoad));
            }
            );
        }

      
    }
}
