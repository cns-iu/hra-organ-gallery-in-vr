using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace HRAOrganGallery
{
    public class SelectionManager : MonoBehaviour
    {
        [SerializeField] private XRRayInteractor _interactor;
        [SerializeField] private GameObject _hovered;
        [SerializeField] private GameObject _selected;
        private List<IHoverResponse> _hoverResponses = new List<IHoverResponse>();
        private List<ISelectionResponse> _selectionResponses = new List<ISelectionResponse>();

        private void OnEnable()
        {
            //Get reference to XRRayInteractor
            _interactor = GetComponent<XRRayInteractor>();

            //Add all classes that implement IHoverResponse to list
            _hoverResponses = GetComponents<IHoverResponse>().ToList();

            //Add all classes that implement ISelectionResponse to list
            _selectionResponses = GetComponents<ISelectionResponse>().ToList();

            //Subscribe to hoverEntered event to trigger selection responses
            _interactor.hoverEntered.AddListener(UpdateHover);

            //Subscribe to hoverEntered event to trigger deselection responses
            _interactor.hoverExited.AddListener(UpdateHoverEnd);

            //Subscribe to trigger pressed event to trigger selection responses
            _interactor.selectEntered.AddListener(UpdateSelection);
        }

        private void UpdateHover(HoverEnterEventArgs args)
        {
            _hovered = args.interactableObject.transform.gameObject;
            for (int i = 0; i < this._hoverResponses.Count; i++)
            {
                this._hoverResponses[i].OnHover(_hovered);
            }
        }

        private void UpdateHoverEnd(HoverExitEventArgs args)
        {
            //_selection = args.interactableObject.transform.gameObject;
            for (int i = 0; i < this._hoverResponses.Count; i++)
            {
                this._hoverResponses[i].OnHoverEnd(_hovered);
            }
            _hovered = null;
        }

        private void UpdateSelection(SelectEnterEventArgs args)
        {
            _selected = args.interactableObject.transform.gameObject;
            for (int i = 0; i < _selectionResponses.Count; i++)
            {
                _selectionResponses[i].OnSelect(_hovered);
            }
        }
    }
}
