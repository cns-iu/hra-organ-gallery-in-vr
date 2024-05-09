using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HRAOrganGallery
{
    public class AdjustColliderToChildren : MonoBehaviour
    {
        private BoxCollider _boxCollider;

        private void Start()
        {
            BoxCollider _boxCollider = GetComponent<BoxCollider>();
            if (_boxCollider != null)
            {
                Bounds bounds = new Bounds(transform.position, Vector3.zero);
                foreach (Renderer renderer in GetComponentsInChildren<Renderer>())
                {
                    bounds.Encapsulate(renderer.bounds);
                }
                _boxCollider.center = bounds.center - transform.position;
                _boxCollider.size = bounds.size;
            }
            else
            {
                Debug.Log($"No box collider attached to {gameObject.name}");
            }

        }
    }
}
