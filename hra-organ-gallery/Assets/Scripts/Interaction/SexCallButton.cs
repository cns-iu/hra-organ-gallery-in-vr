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
        [SerializeField] private bool _hasUserMadeFirstTouch = false;
        [SerializeField] private GameObject uIpanel;
        [SerializeField] private XRSimpleInteractable _interactable;

        private void Awake()
        {
            //SexCallButton.OnClick += (sex) => { TurnOff(sex); };
            
            ReadyMaterial = GetComponent<Renderer>().material;
            Renderer = GetComponent<Renderer>();

            // get collider
            _interactable = GetComponent<XRSimpleInteractable>();
            Collider = GetComponent<BoxCollider>();
            _interactable.colliders.Add(Collider);

            //set active color if on by default
            //if (OrganCaller.Instance.RequestedSex == Feature) ChangeColor(PressedMaterial);
        }

        private void Start()
        {
            SetUpXRInteraction();
        }

        public void SetUpXRInteraction()
        {
            //subscribe to select event
            _interactable.activated.AddListener(
                (ActivateEventArgs args) =>
                {
                    ChangeColor(PressedMaterial);
                    OnClick?.Invoke(Feature);
                }
                );
        }
        
        public void TurnOff(Sex sex)
        {
            if (sex != Feature) ChangeColor(ReadyMaterial);
        }

        private void Update()
        {
            CheckIfLock();
            AutoSwitch();
        }

        public void AutoSwitch()
        {
            if (OrganCaller.Instance.RequestedSex == Feature) ChangeColor(PressedMaterial); else { ChangeColor(ReadyMaterial); }
        }

        public void CheckIfLock()
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
