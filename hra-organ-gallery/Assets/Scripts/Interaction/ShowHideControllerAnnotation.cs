using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace HRAOrganGallery
{
    public class ShowHideControllerAnnotation : MonoBehaviour
    {
        [SerializeField] private Image _annotation;
        [SerializeField] private Image _annotationTurnOn;
        [SerializeField] private TMP_Text _annotationTextTurnOnText;
        [SerializeField] private string _textWhenTurnedOn = "Click <b>\"A\"</b> button to show annotation";
        [SerializeField] private InputActionReference _annotationActionReference;

        private void Awake()
        {
            _annotationTurnOn.enabled = false;
            _annotationTextTurnOnText.text = "";

            _annotationActionReference.action.performed += UpdateAnnotationDisplay;
        }

        private void UpdateAnnotationDisplay(InputAction.CallbackContext context)
        {

            // handle UI annotations
            _annotation.enabled = !_annotation.enabled;

            // handle note to turn on annotations
            _annotationTurnOn.enabled = !_annotation.enabled;
            _annotationTextTurnOnText.text = _annotationTurnOn.enabled ? _textWhenTurnedOn : "";

        }
    }
}
