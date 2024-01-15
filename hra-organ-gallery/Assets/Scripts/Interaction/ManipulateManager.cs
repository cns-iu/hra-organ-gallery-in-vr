using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Transformers;

namespace HRAOrganGallery
{
    /// <summary>
    /// A class that handles turning XR interactables on and off
    /// </summary>
    public class ManipulateManager : MonoBehaviour, IHandleSwitchStateChange
    {

        [SerializeField] private XRGeneralGrabTransformer _generalGrab;
        [SerializeField] private XRGrabInteractable _grab;

        public void HandleSwitchStateChange()
        {
            UserInputStateManager.OnStateChanged += (UserInputState newState) =>
            {
                bool isActive = newState == UserInputState.Movement;

                _generalGrab.enabled = isActive;
                _grab.enabled = isActive;
            };
        }

        private void Awake()
        {
            HandleSwitchStateChange();
        }
    }
}
