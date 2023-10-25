using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace HRAOrganGallery
{
    public class OnOffButton : MonoBehaviour
    {
        [field: SerializeField] public Material PressedMaterial { get; set; }
        [field: SerializeField] public Material ReadyMaterial { get; set; }

        [SerializeField] private List<GameObject> ToggleThis;

        private void Awake()
        {
            GetComponent<XRSimpleInteractable>().hoverEntered.AddListener(
                (HoverEnterEventArgs args) =>
                {
                    //toggle game object (parent) on and off
                    for (int i = 0; i < ToggleThis.Count; i++)
                    {
                        bool active = !ToggleThis[i].gameObject.activeSelf;
                        ToggleThis[i].SetActive(active);

                        //set material
                        Material mat = active ? PressedMaterial : ReadyMaterial;
                        ChangeColor(mat);
                    }
                }
                );
        }

        private void ChangeColor(Material mat)
        {
            GetComponent<Renderer>().material = mat;
        }
    }
}
