using Assets.Scripts.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

namespace HRAOrganGallery
{
    public class SexCallButton : MonoBehaviour, IKeyboardButton<Sex, Sex>, IPauseCollision
    {
        public static event Action<Sex> OnClick;
        public static event Action<bool> OnCollideWithPriorityLayer;
        [field: SerializeField] public BoxCollider Collider { get; set; }
        [field: SerializeField] public Sex Feature { get; set; }

        [field: SerializeField] public Material PressedMaterial { get; set; }
        [field: SerializeField] public Material DisabledMaterial { get; set; }
        [field: SerializeField] public Material ReadyMaterial { get; set; }

        [field: SerializeField] public Renderer Renderer { get; set; }

        [SerializeField] private SexCallButton other;

        [SerializeField] private bool _locked = true;
        [SerializeField] private XRBaseInteractable _interactable;

        private IKeyboardHover _keyboardHoverResponse;
        [SerializeField] private ParticleSystem _player;

        private void Awake()
        {
            _interactable = GetComponent<XRBaseInteractable>();
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
                (HoverEnterEventArgs args) => { _keyboardHoverResponse.OnHoverEnter(); OnCollideWithPriorityLayer?.Invoke(true); }
                );

            //subscribe to hover exit event
            _interactable.hoverExited.AddListener(
                (HoverExitEventArgs args) => { _keyboardHoverResponse.OnHoverExit(); OnCollideWithPriorityLayer?.Invoke(false); }
                );
        }

        private void SetVisibility(OrganData data)
        {
            //determine if locked
            _locked = OrganCaller.Instance.FemaleOnlyOrgans.Contains(OrganCaller.Instance.RequestedOrgan) | OrganCaller.Instance.MaleOnlyOrgans.Contains(OrganCaller.Instance.RequestedOrgan) | OrganCaller.Instance.RequestedOrgan == "";

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
            if (OrganCaller.Instance.RequestedSex == Feature) ChangeColor(PressedMaterial); else { ChangeColor(ReadyMaterial); }
        }

        private void ChangeColor(Material mat)
        {
            Renderer.material = mat;
        }
    }
}
