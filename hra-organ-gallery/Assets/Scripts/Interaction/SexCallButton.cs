using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HRAOrganGallery
{
    public class SexCallButton : MonoBehaviour, IKeyboardButton<Sex, Sex>
    {
        public static event Action<Sex> OnClick;
        [field: SerializeField] public BoxCollider Collider { get; set; }
        [field: SerializeField] public Sex Feature { get; set; }

        [field: SerializeField] public Material ActiveMaterial { get; set; }
        [field: SerializeField] public Material InactiveMaterial { get; set; }

        [field: SerializeField] public Renderer Renderer { get; set; }

        [SerializeField] private SexCallButton other;

        //could be singleton but keeping it as is because we may have more than one organ platform/caller
        [SerializeField] private OrganCaller caller;

        [SerializeField] private bool _locked = false;

        private void Awake()
        {


            SexCallButton.OnClick += (sex) => { TurnOff(sex);};
            Collider = GetComponent<BoxCollider>();
            InactiveMaterial = GetComponent<Renderer>().material;
            Renderer = GetComponent<Renderer>();

            //set active color if on by default
            if (caller.GetComponent<OrganCaller>().DefaultSex == Feature) ChangeColor(ActiveMaterial);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_locked) return;
            ChangeColor(ActiveMaterial);
            OnClick?.Invoke(Feature);
        }

        public void TurnOff(Sex sex)
        {
            if (sex != Feature) ChangeColor(InactiveMaterial);
        }

        private void Update()
        {
            CheckIfLock();
        }

        public void CheckIfLock()
        {
            _locked = caller.FemaleOnlyOrgans.Contains(caller.RequestedOrgan) | caller.MaleOnlyOrgans.Contains(caller.RequestedOrgan);

        }

        private void ChangeColor(Material mat)
        {
            Renderer.material = mat;
        }
    }
}
