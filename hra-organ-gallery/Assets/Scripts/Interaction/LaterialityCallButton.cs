using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HRAOrganGallery
{
    public class LaterialityCallButton : MonoBehaviour, IKeyboardButton<Laterality, Laterality>
    {
        public static event Action<Laterality> OnClick;
        [field: SerializeField] public BoxCollider Collider { get; set; }
        [field: SerializeField] public Laterality Feature { get; set; }
        [field: SerializeField] public Material ActiveMaterial { get; set; }
        [field: SerializeField] public Material InactiveMaterial { get; set; }

        [field: SerializeField] public Renderer Renderer { get; set; }

        [SerializeField] private LaterialityCallButton other;

        //could be singleton but keeping it as is because we may have more than one organ platform/caller
        [SerializeField] private OrganCaller caller;

        [SerializeField] private bool _locked = false;

        private void Awake()
        {
            LaterialityCallButton.OnClick += (lat) => { TurnOff(lat); };
            Collider = GetComponent<BoxCollider>();
            InactiveMaterial = GetComponent<Renderer>().material;
            Renderer = GetComponent<Renderer>();

            //set active color if on by default
            if (caller.GetComponent<OrganCaller>().DefaultLaterality == Feature) ChangeColor(ActiveMaterial);
        }

        private void OnTriggerEnter(Collider other)
        {
            ChangeColor(ActiveMaterial);
            OnClick?.Invoke(Feature);
        }

        private void Update()
        {
            CheckIfLock();
        }

        public void CheckIfLock()
        {
            _locked = caller.FemaleOnlyOrgans.Contains(caller.RequestedOrgan) | caller.MaleOnlyOrgans.Contains(caller.RequestedOrgan);

        }

        public void TurnOff(Laterality lat)
        {
            if (lat != Feature) ChangeColor(InactiveMaterial);
        }

        private void ChangeColor(Material mat)
        {
            Renderer.material = mat;
        }
    }
}
