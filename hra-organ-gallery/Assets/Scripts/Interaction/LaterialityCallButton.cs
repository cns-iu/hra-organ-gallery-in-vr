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
        [field: SerializeField] public Material ReadyMaterial { get; set; }

        [field: SerializeField] public Renderer Renderer { get; set; }

        [SerializeField] private LaterialityCallButton other;

        //could be singleton but keeping it as is because we may have more than one organ platform/caller
        [SerializeField] private OrganCaller caller;

        [SerializeField] private bool _locked = true;
        [SerializeField] private GameObject uIpanel;
        [SerializeField] private XRSimpleInteractable _interactable;


        private void Awake()
        {
            LaterialityCallButton.OnClick += (lat) => { TurnOff(lat); };
            Collider = GetComponent<BoxCollider>();
            ReadyMaterial = GetComponent<Renderer>().material;
            Renderer = GetComponent<Renderer>();

            //set active color if on by default
            if (caller.GetComponent<OrganCaller>().RequestedLaterality == Feature) ChangeColor(PressedMaterial);
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
                    ChangeColor(PressedMaterial);
                    OnClick?.Invoke(Feature);
                }
                );
        }
        private void OnValidate()
        {
            if (_interactable.colliders.Count == 0) _interactable.colliders.Add(GetComponent<BoxCollider>());
        }

        private void Update()
        {
            CheckIfLock();
        }

        public void CheckIfLock()
        {
            _locked = !caller.TwoSidedOrgans.Contains(caller.RequestedOrgan) | caller.RequestedOrgan == "";
            uIpanel.SetActive(_locked);
        }

        public void TurnOff(Laterality lat)
        {
            if (lat != Feature) ChangeColor(ReadyMaterial);
        }

        private void ChangeColor(Material mat)
        {
            Renderer.material = mat;
        }
    }
}
