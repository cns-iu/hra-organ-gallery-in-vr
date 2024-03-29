using Assets.Scripts.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

namespace HRAOrganGallery
{
    public class LaterialityCallButton : MonoBehaviour, IKeyboardButton<Laterality, Laterality>
    {
        public static event Action<Laterality> OnClick;
        [field: SerializeField] public BoxCollider Collider { get; set; }
        [field: SerializeField] public Laterality Feature { get; set; }
        [field: SerializeField] public Material PressedMaterial { get; set; }
        [field: SerializeField] public Material DisabledMaterial { get; set; }
        [field: SerializeField] public Material ReadyMaterial { get; set; }

        [field: SerializeField] public Renderer Renderer { get; set; }

        [SerializeField] private LaterialityCallButton other;
        [SerializeField] private bool _locked = true;
        [SerializeField] private XRSimpleInteractable _interactable;
        private IKeyboardHover _keyboardHoverResponse;


        private void Awake()
        {
            _interactable.GetComponent<XRSimpleInteractable>();
            Collider = GetComponent<BoxCollider>();
            _interactable.colliders.Add(Collider);
            Renderer = GetComponent<Renderer>();
            _keyboardHoverResponse = GetComponentInChildren<IKeyboardHover>();

            OrganCaller.OnOrganPicked += SetVisibility;
            OrganCaller.OnOrganPicked += AutoSwitch;
        }

        private void OnDestroy()
        {
            OrganCaller.OnOrganPicked -= SetVisibility;
            OrganCaller.OnOrganPicked -= AutoSwitch;
        }


        private void Start()
        {
            SetUpXRInteraction();
        }

        public void SetUpXRInteraction()
        {
            //subscribe to select event
            _interactable.selectEntered.AddListener(
                (SelectEnterEventArgs args) =>
                {
                    ChangeColor(PressedMaterial);
                    OnClick?.Invoke(Feature);
                }
                );

            //subscribe to hover enter event
            _interactable.hoverEntered.AddListener(
                (HoverEnterEventArgs args) => { _keyboardHoverResponse.OnHoverEnter(); }
                );

            //subscribe to hover exit event
            _interactable.hoverExited.AddListener(
                (HoverExitEventArgs args) => { _keyboardHoverResponse.OnHoverExit(); }
                );
        }

        private void SetVisibility(OrganData data)
        {
            //determine if locked
            _locked = !OrganCaller.Instance.TwoSidedOrgans.Contains(OrganCaller.Instance.RequestedOrgan) | OrganCaller.Instance.RequestedOrgan == "";

            //set interactable
            _interactable.enabled = !_locked;

            //set color
            if (_locked) ChangeColor(DisabledMaterial);
            else
            {
                ChangeColor(ReadyMaterial);
            }


        }

        public void AutoSwitch(OrganData data)
        {
            if (_locked) return;
            if (OrganCaller.Instance.RequestedLaterality == Feature) ChangeColor(PressedMaterial); else { ChangeColor(ReadyMaterial); }
        }

        private void ChangeColor(Material mat)
        {
            Renderer.material = mat;
        }
    }
}
