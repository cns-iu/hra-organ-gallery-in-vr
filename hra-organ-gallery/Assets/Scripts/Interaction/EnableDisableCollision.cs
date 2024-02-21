using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HRAOrganGallery
{
    public class EnableDisableCollision : MonoBehaviour
    {
        [SerializeField] BoxCollider _grabCylinderCollider;
        [SerializeField] private bool _isCollidingWithPriority = false;

        private void Awake()
        {
            AfterInteractResetOrgan.OnCollideWithPriorityLayer += EnableDisable;
        }

        private void OnDestroy()
        {
            AfterInteractResetOrgan.OnCollideWithPriorityLayer -= EnableDisable;
        }

        private void EnableDisable(bool collides)
        {
            _isCollidingWithPriority = collides;
            _grabCylinderCollider.enabled = !_isCollidingWithPriority;
            Debug.Log($"{_grabCylinderCollider} was set to {_grabCylinderCollider.enabled}.");
        }
    }
}
