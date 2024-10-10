using System;
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
        public static Action<float> OnScaleChanged;

        private Vector3 _originalScale;
        private Vector3 _previousScale;

        private void Awake()
        {
            //implement singleton 
            if (Instance != null)
                Destroy(gameObject);
            else
                Instance = this;

            DontDestroyOnLoad(gameObject);

            //get original scale
            _originalScale = transform.localScale;
            _previousScale = transform.localScale;
        }

        private void Update()
        {
            if (_previousScale != transform.localScale)
            {
                float scalingFactor = transform.localScale.x / _originalScale.x;
                OnScaleChanged?.Invoke(scalingFactor);
                _previousScale = transform.localScale;
            }

        }
    }
}
