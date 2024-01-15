using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HRAOrganGallery
{
    /// <summary>
    /// A class to turn on and off tissue block explode components on the probing sphere
    /// </summary>
    public class ExplodeTurnOnOffManager : MonoBehaviour, IHandleSwitchStateChange
    {
        [SerializeField] TissueBlockExploder _exploder;
        [SerializeField] AdjustSphereSize _adjustSphereSize;

        public void HandleSwitchStateChange()
        {
            UserInputStateManager.OnStateChanged += (UserInputState newState) =>
            {
                bool isActive = newState == UserInputState.TissueBlockExplode;
                _exploder.enabled = isActive;
                _adjustSphereSize.enabled = isActive;
            };
        }

        private void Awake()
        {
            HandleSwitchStateChange();
        }
    }
}
