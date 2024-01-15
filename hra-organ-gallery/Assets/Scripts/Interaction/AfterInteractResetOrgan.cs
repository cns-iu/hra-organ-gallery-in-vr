using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

namespace HRAOrganGallery
{
    public class AfterInteractResetOrgan : MonoBehaviour
    {
        public static event Action OnOrganResetClicked;

        [field: SerializeField] public Material PressedMaterial { get; set; }
        [field: SerializeField] public Material ReadyMaterial { get; set; }

        private float lightTime = 2f;

        private void Awake()
        {
            GetComponent<XRSimpleInteractable>().activated.AddListener(
                (ActivateEventArgs args) => { 
                    OnOrganResetClicked.Invoke();
                    StartCoroutine(LightUp());
                }
                );
        }

        private IEnumerator LightUp() {
            float elapsedTime = 0f;

            GetComponent<MeshRenderer>().material = PressedMaterial;
            while (elapsedTime < lightTime)
            {
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            GetComponent<MeshRenderer>().material = ReadyMaterial;
        }

    }
}
