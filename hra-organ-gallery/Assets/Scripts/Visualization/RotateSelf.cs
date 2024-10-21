using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HRAOrganGallery
{
    public class RotateSelf : MonoBehaviour
    {
        [SerializeField] private Vector3 newRotation = new Vector3(-90, 0, 0);

        private void Awake()
        {
            transform.Rotate(newRotation);
        }
    }
}
