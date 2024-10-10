using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Transformers;

namespace HRAOrganGallery
{
    public class BroadcastGrabCylinderScale : MonoBehaviour
    {
        public static BroadcastGrabCylinderScale Instance;
        public Vector3 Scale { get {
                return new Vector3(
                    transform.localScale.x / _originalSize.x,
                    transform.localScale.y / _originalSize.y,
                    transform.localScale.z / _originalSize.z
                    );
            } }

        private Vector3 _originalSize;

        private void Awake()
        {
            //implement singleton 
            if (Instance != null)
                Destroy(gameObject);
            else
                Instance = this;

            DontDestroyOnLoad(gameObject);

            //get original scale
            _originalSize = transform.localScale;
        }
    }
}
