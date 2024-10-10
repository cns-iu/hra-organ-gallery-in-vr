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

        private void Update()
        {
            RescaleOnGrabCylinderScale();
        }

        private void RescaleOnGrabCylinderScale()
        {
            Debug.Log(BroadcastGrabCylinderScale.Instance.Scale);
            Vector3 adjustedScale = new Vector3(
                transform.localScale.x * BroadcastGrabCylinderScale.Instance.Scale.x,
                transform.localScale.y * BroadcastGrabCylinderScale.Instance.Scale.y,
                transform.localScale.z * BroadcastGrabCylinderScale.Instance.Scale.z
                );
            
            transform.localScale = adjustedScale;
        }
    }

}
