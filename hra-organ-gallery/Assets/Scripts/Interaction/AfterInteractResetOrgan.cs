using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

namespace HRAOrganGallery
{
    public class AfterInteractResetOrgan : MonoBehaviour, IPauseCollision
    {
        public static event Action OnOrganResetClicked;

        public static event Action<bool> OnCollideWithPriorityLayer;

        [field: SerializeField] public Material PressedMaterial { get; set; }
        [field: SerializeField] public Material ReadyMaterial { get; set; }

        private float lightTime = 2f;
        private IKeyboardHover _keyboardHoverResponse;

        private void Awake()
        {
            //get keyboard hover response
            _keyboardHoverResponse = GetComponentInChildren<IKeyboardHover>();

            GetComponent<XRBaseInteractable>().selectEntered.AddListener(
                (SelectEnterEventArgs args) =>
                {
                    OnOrganResetClicked.Invoke();
                    StartCoroutine(LightUp());
                }
                );

            //subscribe to hover enter event
            GetComponent<XRBaseInteractable>().hoverEntered.AddListener(
                (HoverEnterEventArgs args) =>
                {
                    _keyboardHoverResponse.OnHoverEnter();
                    OnCollideWithPriorityLayer?.Invoke(true);
                }
                );

            //subscribe to hover exit event
            GetComponent<XRBaseInteractable>().hoverExited.AddListener(
                (HoverExitEventArgs args) =>
                {
                    _keyboardHoverResponse.OnHoverExit();
                    OnCollideWithPriorityLayer?.Invoke(false);
                }
                );
        }

        private IEnumerator LightUp()
        {
            float elapsedTime = 0f;

            GetComponent<MeshRenderer>().material = PressedMaterial;
            while (elapsedTime < lightTime)
            {
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            GetComponent<MeshRenderer>().material = ReadyMaterial;
        }

        public void IsCollidingWithPriority(bool isColliding)
        {

        }
    }
}
