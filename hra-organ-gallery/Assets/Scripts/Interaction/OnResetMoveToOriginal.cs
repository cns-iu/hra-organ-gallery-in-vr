using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace HRAOrganGallery
{
    public class OnResetMoveToOriginal : MonoBehaviour
    {
        private float resetTime = 2f;
        private Vector3 transformDefaultPosition;
        private Quaternion transformDefaultRotation;
        private Vector3 transformDefaultScale;

        private void Awake()
        {
            transformDefaultPosition = transform.position;
            transformDefaultRotation = transform.rotation;
            transformDefaultScale = transform.localScale;
        }

        private void OnEnable()
        {
            AfterInteractResetOrgan.OnOrganResetClicked += HandleReset;
        }

        private void OnDestroy()
        {
            AfterInteractResetOrgan.OnOrganResetClicked -= HandleReset;
        }

        private void HandleReset()
        {
            StartCoroutine(SmoothResetGrabber());
        }
        private IEnumerator SmoothResetGrabber()
        {
            Vector3 startPosition = transform.position;
            Quaternion startRotation = transform.rotation;
            Vector3 startScale = transform.localScale;

            float elapsedTime = 0f;

            while (elapsedTime < resetTime)
            {
                float t = elapsedTime / resetTime;
                transform.position = Vector3.Slerp(startPosition, transformDefaultPosition, t);
                transform.rotation = Quaternion.Slerp(startRotation, transformDefaultRotation, t);
                transform.localScale = Vector3.Slerp(startScale, transformDefaultScale, t);

                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }
    }
}
