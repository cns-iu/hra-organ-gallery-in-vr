using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

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
        [field: SerializeField] public BoxCollider Collider { get; set; }

        [field: SerializeField] public Material ActiveMaterial { get; set; }
        [field: SerializeField] public Material InactiveMaterial { get; set; }
        [field: SerializeField] public Renderer Renderer { get; set; }


        [SerializeField] private List<OrganCallButton> others;

        private void Awake()
        {
            OrganCallButton.OnCLick += (iris) => { TurnOff(iris); };
            Collider = GetComponent<BoxCollider>();
            InactiveMaterial = GetComponent<Renderer>().material;
            Renderer = GetComponent<Renderer>();
        }

        private void OnTriggerEnter(Collider other)
        {
            ChangeColor(ActiveMaterial);
            OnCLick?.Invoke(Feature);
        }

        public void TurnOff(List<string> iris)
        {
            if (iris != Feature) ChangeColor(InactiveMaterial);
        }

        private void ChangeColor(Material mat)
        {
            Renderer.material = mat;
        }

    }
}
