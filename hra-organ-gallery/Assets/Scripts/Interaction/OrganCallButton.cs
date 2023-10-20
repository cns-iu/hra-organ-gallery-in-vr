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


    public class OrganCallButton : MonoBehaviour, IKeyboardButton<List<string>, List<string>>
    {
        public static event Action<List<string>> OnCLick;
        [field: SerializeField] public List<string> Feature { get; set; }

        [field: SerializeField] public Material PressedMaterial { get; set; }
        [field: SerializeField] public Material ReadyMaterial { get; set; }

        [field: SerializeField] public Material DisabledMaterial { get; set; }

        [field: SerializeField] public Renderer Renderer { get; set; }


        [SerializeField] private List<OrganCallButton> others;
        [SerializeField] private XRSimpleInteractable _interactable;

        private void Awake()
        {
            OrganCallButton.OnCLick += (iris) => { TurnOff(iris); };
            ReadyMaterial = GetComponent<Renderer>().material;
            Renderer = GetComponent<Renderer>();

            //get ref to simple interactable
            _interactable = GetComponent<XRSimpleInteractable>();

            //get collider
            _interactable.colliders.Add(GetComponent<BoxCollider>());
        }

        private void Start()
        {
            //subscribe to hover event
            _interactable.hoverEntered.AddListener(
                (HoverEnterEventArgs args) =>
                {
                    ChangeColor(PressedMaterial);
                    OnCLick?.Invoke(Feature);
                }
                );
        }

        private void OnValidate()
        {
            if (_interactable.colliders.Count == 0) _interactable.colliders.Add(GetComponent<BoxCollider>());
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
