using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

namespace HRAOrganGallery
{
    /// <summary>
    /// A class to describe an organ call button on the keyboard
    /// </summary>
    /// 


    public class OrganCallButton : MonoBehaviour, IKeyboardButton<List<string>, List<string>>, IPauseCollision
    {
        public static event Action<List<string>> OnClick;
        public static event Action<bool> OnCollideWithPriorityLayer;
        [field: SerializeField] public List<string> Feature { get; set; }

        [field: SerializeField] public Material PressedMaterial { get; set; }
        [field: SerializeField] public Material ReadyMaterial { get; set; }

        [field: SerializeField] public Material DisabledMaterial { get; set; }

        [field: SerializeField] public Renderer Renderer { get; set; }
        
        [SerializeField] private List<OrganCallButton> others;
        [SerializeField] private XRBaseInteractable _interactable;
        private IKeyboardHover _keyboardHoverResponse;


        private void Awake()
        {
            OrganCallButton.OnClick += (iris) => { TurnOff(iris); };
            ReadyMaterial = GetComponent<Renderer>().material;
            Renderer = GetComponent<Renderer>();
            _keyboardHoverResponse = GetComponentInChildren<IKeyboardHover>();
        }

        private void Start()
        {
            SetUpXRInteraction();
        }

        public void SetUpXRInteraction()
        {
            //get ref to simple interactable
            _interactable = GetComponent<XRBaseInteractable>();

            //get collider
            _interactable.colliders.Add(GetComponent<BoxCollider>());

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

        public void TurnOff(List<string> iris)
        {
            if (iris != Feature) ChangeColor(ReadyMaterial);
        }

        private void ChangeColor(Material mat)
        {
            Renderer.material = mat;
        }

    }
}
