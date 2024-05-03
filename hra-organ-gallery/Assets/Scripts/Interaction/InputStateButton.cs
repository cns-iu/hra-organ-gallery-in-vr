using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

namespace HRAOrganGallery
{
    public class InputStateButton : MonoBehaviour, IStateSelect, IPauseCollision
    {
        public static event Action<bool> OnCollideWithPriorityLayer;

        [Header("State")]
        [SerializeField] private UserInputState _state;

        [Header("Materials")]
        [SerializeField] private Material _readyMaterial;
        [SerializeField] private Material _pressedMaterial;
        [SerializeField] private Renderer _renderer;

        [Header("Interaction")]
        [SerializeField] private XRBaseInteractable _interactable;
        [SerializeField] private IKeyboardHover _keyboardHoverResponse;

        private void Awake()
        {
            //get materials
            _renderer = GetComponent<Renderer>();

            // get keyboard hover response
            _keyboardHoverResponse = GetComponent<IKeyboardHover>();

            //get interactable
            _interactable = GetComponent<XRBaseInteractable>();

            //subscribe to state change event for delecting
            UserInputStateManager.OnStateChanged += OnDeselect;

            //subscribe to hover enter event
            _interactable.hoverEntered.AddListener(
                (HoverEnterEventArgs args) =>
                {
                    _keyboardHoverResponse.OnHoverEnter();
                    OnCollideWithPriorityLayer(true);
                }
                );

            //subscribe to hover exit event
            _interactable.hoverExited.AddListener(
                (HoverExitEventArgs args) =>
                {
                    _keyboardHoverResponse.OnHoverExit();
                    OnCollideWithPriorityLayer(false);
                }
                );

        }
        public void OnSelect()
        {
            _renderer.material = _pressedMaterial;
            UserInputStateManager.Instance.State = _state;
        }

        public void OnDeselect(UserInputState state)
        {
            if (_state != state) { _renderer.material = _readyMaterial; }
        }
    }
}
