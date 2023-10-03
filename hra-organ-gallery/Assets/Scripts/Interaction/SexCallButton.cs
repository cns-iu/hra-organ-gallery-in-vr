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

        [field: SerializeField] public bool IsLocked { get; set; }

        [SerializeField] private SexCallButton other;

        private void Awake()
        {
            SexCallButton.OnClick += (sex) => { TurnOff(sex); };
            Collider = GetComponent<BoxCollider>();
            InactiveMaterial = GetComponent<Renderer>().material;
            Renderer = GetComponent<Renderer>();
        }

        private void OnTriggerEnter(Collider other)
        {
            ChangeColor(ActiveMaterial);
            OnClick?.Invoke(Feature);
        }

        public void TurnOff(Sex sex)
        {
            if (sex != Feature) ChangeColor(InactiveMaterial);
        }

        private void ChangeColor(Material mat)
        {
            Renderer.material = mat;
        }
    }
}
