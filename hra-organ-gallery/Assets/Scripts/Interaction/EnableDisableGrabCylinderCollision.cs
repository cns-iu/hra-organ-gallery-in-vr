using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace HRAOrganGallery
{
    public class EnableDisableGrabCylinderCollision : MonoBehaviour
    {
        [SerializeField] private int layerMaskIntToIgnore = 2;
        private XRRayInteractor _interactor;
        private LayerMask _defaultLayerMask;

        private void Awake()
        {
            _interactor = GetComponent<XRRayInteractor>();
            _defaultLayerMask = _interactor.raycastMask;
        }
        private void OnEnable()
        {
            AfterInteractResetOrgan.OnCollideWithPriorityLayer += AdjustRaycastMask;
            InputStateButton.OnCollideWithPriorityLayer += AdjustRaycastMask;
            OrganCallButton.OnCollideWithPriorityLayer += AdjustRaycastMask;
            LaterialityCallButton.OnCollideWithPriorityLayer += AdjustRaycastMask;
            SexCallButton.OnCollideWithPriorityLayer += AdjustRaycastMask;
        }

        private void OnDestroy()
        {
            AfterInteractResetOrgan.OnCollideWithPriorityLayer -= AdjustRaycastMask;
            InputStateButton.OnCollideWithPriorityLayer -= AdjustRaycastMask;
            OrganCallButton.OnCollideWithPriorityLayer -= AdjustRaycastMask;
            LaterialityCallButton.OnCollideWithPriorityLayer -= AdjustRaycastMask;
            SexCallButton.OnCollideWithPriorityLayer -= AdjustRaycastMask;
        }

        private void AdjustRaycastMask(bool isCollidingWithPriorityLayer)
        {
            //change layer mask to include or exclude the IgnoreRaycast layer
            // Bit shift the index of the layer (3) to get a bit mask
            int layerMask = 1 << layerMaskIntToIgnore;

            // This would cast rays only against colliders in layer 3.
            // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
            _interactor.raycastMask = isCollidingWithPriorityLayer ? ~layerMask : _defaultLayerMask;
        }
    }
}
