using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace HRAOrganGallery
{
    public class ElevatorButton : MonoBehaviour, IPauseCollision
    {
        public static event Action<bool> OnCollideWithPriorityLayer;

        [SerializeField] private XRBaseInteractable _interactable;
        private IKeyboardHover _keyboardHoverResponse;
        

        private void Awake()
        {
            SetUpXRInteraction();

            _keyboardHoverResponse = GetComponentInChildren<IKeyboardHover>();
        }

        private void SetUpXRInteraction()
        {
            _interactable = GetComponent<XRBaseInteractable>();
            _interactable.colliders[0] = GetComponent<Collider>();

            //subscribe to hover enter event
            _interactable.hoverEntered.AddListener(
                (HoverEnterEventArgs args) => { _keyboardHoverResponse.OnHoverEnter(); OnCollideWithPriorityLayer?.Invoke(true); }
                );

            //subscribe to hover exit event
            _interactable.hoverExited.AddListener(
                (HoverExitEventArgs args) => { _keyboardHoverResponse.OnHoverExit(); OnCollideWithPriorityLayer?.Invoke(false); }
                );
        }
    }
}
