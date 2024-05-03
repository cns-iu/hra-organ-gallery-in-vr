using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace HRAOrganGallery
{
    public class ElevatorButton : MonoBehaviour
    {
        [SerializeField] private XRSimpleInteractable _interactable;

        private void Awake()
        {
            _interactable = GetComponent<XRSimpleInteractable>();
            _interactable.colliders[0] = GetComponent<BoxCollider>();
        }
    }
}
