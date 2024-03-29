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
            InputStateButton.OnCollideWithPriorityLayer += EnableDisable;
        }

        private void OnDestroy()
        {
            AfterInteractResetOrgan.OnCollideWithPriorityLayer -= EnableDisable;
            InputStateButton.OnCollideWithPriorityLayer -= EnableDisable;
        }

        private void EnableDisable(bool collides)
        {
            _isCollidingWithPriority = collides;
            
            //ignore collision if we are colliding with a high priority collider, e.g., on the keyboard
            Physics.IgnoreCollision(_grabCylinderCollider, GetComponent<Collider>(), _isCollidingWithPriority);

            //old code to turn off entire collider --too brutish
            //_grabCylinderCollider.enabled = !_isCollidingWithPriority;

        }
    }
}
