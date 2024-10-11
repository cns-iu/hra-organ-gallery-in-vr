using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace HRAOrganGallery
{
    /// <summary>
    /// A class to set the scale display on a ScaleCube based on the grab cyliner
    /// </summary>
    public class ScaleHandler : MonoBehaviour
    {
        private Vector3 originalScale;

        private void OnEnable()
        {
            BroadcastGrabCylinderScale.OnScaleChanged += RescaleOnGrabCylinderScale;
        }

        private void OnDestroy()
        {
            BroadcastGrabCylinderScale.OnScaleChanged -= RescaleOnGrabCylinderScale;
        }

        private void Awake()
        {
            originalScale = transform.localScale;
        }

        private void RescaleOnGrabCylinderScale(float scalingFactor)
        {
            transform.localScale = new Vector3(
                originalScale.x * scalingFactor,
                originalScale.y * scalingFactor,
                originalScale.z * scalingFactor
                );
        }
    }

}
