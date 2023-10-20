using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

namespace HRAOrganGallery
{
    public class SexCallButton : MonoBehaviour, IKeyboardButton<Sex, Sex>
    {
        public static event Action<Sex> OnClick;
        [field: SerializeField] public BoxCollider Collider { get; set; }
        [field: SerializeField] public Sex Feature { get; set; }

        [field: SerializeField] public Material PressedMaterial { get; set; }
        [field: SerializeField] public Material ReadyMaterial { get; set; }

        [field: SerializeField] public Renderer Renderer { get; set; }

        [SerializeField] private SexCallButton other;

        [SerializeField] private bool _locked = true;
        [SerializeField] private GameObject uIpanel;
        [SerializeField] private XRSimpleInteractable _interactable;

        private void Awake()
        {
            //SexCallButton.OnClick += (sex) => { TurnOff(sex); };
            Collider = GetComponent<BoxCollider>();
            ReadyMaterial = GetComponent<Renderer>().material;
            Renderer = GetComponent<Renderer>();

            //set active color if on by default
            //if (OrganCaller.Instance.RequestedSex == Feature) ChangeColor(PressedMaterial);
        }

        private void Start()
        {
            SetUpXRInteraction();
        }

        public void SetUpXRInteraction()
        {
            //subscribe to hover event
            _interactable.hoverEntered.AddListener(
                (HoverEnterEventArgs args) =>
                {
                    if (_locked) return;
                    ChangeColor(PressedMaterial);
                    OnClick?.Invoke(Feature);
                }
                );
        }
        private void OnValidate()
        {
            if (_interactable.colliders.Count == 0) _interactable.colliders.Add(GetComponent<BoxCollider>());
        }

        public void TurnOff(Sex sex)
        {
            if (sex != Feature) ChangeColor(ReadyMaterial);
        }

        private void Update()
        {
            //MustLock();
            AutoSwitch();
        }

        public void AutoSwitch()
        {
            if (OrganCaller.Instance.RequestedSex == Feature) ChangeColor(PressedMaterial); else { ChangeColor(ReadyMaterial); }
        }

        public void MustLock()
        {
            _locked = OrganCaller.Instance.FemaleOnlyOrgans.Contains(OrganCaller.Instance.RequestedOrgan) | OrganCaller.Instance.MaleOnlyOrgans.Contains(OrganCaller.Instance.RequestedOrgan) | OrganCaller.Instance.RequestedOrgan == "";
            uIpanel.SetActive(_locked);
        }

        private void ChangeColor(Material mat)
        {
            Renderer.material = mat;
        }
    }
}
