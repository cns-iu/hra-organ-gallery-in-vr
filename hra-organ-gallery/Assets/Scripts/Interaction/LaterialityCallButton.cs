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

        [field: SerializeField] public bool IsLocked { get; set; }

        [SerializeField] private LaterialityCallButton other;

        private void Awake()
        {
            LaterialityCallButton.OnClick += (lat) => { TurnOff(lat); };
            Collider = GetComponent<BoxCollider>();
            InactiveMaterial = GetComponent<Renderer>().material;
            Renderer = GetComponent<Renderer>();
        }

        private void OnTriggerEnter(Collider other)
        {
            IsLocked = true;
            ChangeColor(ActiveMaterial);
            OnClick?.Invoke(Feature);
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
