using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace HRAOrganGallery
{
    /// <summary>
    /// A class that handles turning XR locmotion on and off
    /// </summary>
    public class MoveManager : MonoBehaviour
    {
        [SerializeField] private List<LocomotionProvider> _providers;
        private void Awake()
        {
            UserInputStateManager.OnStateChanged += (UserInputState newState) =>
            {
                _providers.ForEach(p =>
                {
                    p.enabled = (newState == UserInputState.Movement);
                });
            };
        }
    }
}
