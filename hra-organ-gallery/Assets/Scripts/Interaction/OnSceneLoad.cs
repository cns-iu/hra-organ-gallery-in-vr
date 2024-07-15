using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HRAOrganGallery
{
    public class OnSceneLoad : MonoBehaviour
    {
        private void OnEnable()
        {
            SceneManagementUtils.OnLoadSceneStart += SetOverlayActive;
        }

        private void OnDestroy()
        {
            SceneManagementUtils.OnLoadSceneStart -= SetOverlayActive;
        }

        private void SetOverlayActive(bool hasStartedLoading) { 
            GetComponent<Camera>().enabled = hasStartedLoading;
        }
    }
}
