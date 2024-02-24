using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace HRAOrganGallery
{
    public class OnHoverParticles : MonoBehaviour
    {
        private XRSimpleInteractable _interactable;
        private ParticleSystem _particleSystem;

        private void Awake()
        {
            _interactable = GetComponent<XRSimpleInteractable>();
            _particleSystem = GetComponentInChildren<ParticleSystem>();

            _interactable.hoverEntered.AddListener(
                (HoverEnterEventArgs args) => { _particleSystem.Play(); }
                );

            _interactable.hoverExited.AddListener(
                (HoverExitEventArgs args) => { _particleSystem.Stop(); }
                );
        }
    }
}
