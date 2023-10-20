using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace HRAOrganGallery
{
    public class OnOffButton : MonoBehaviour
    {
        [SerializeField] private List<GameObject> ToggleThis;

        private void Awake()
        {
            GetComponent<XRSimpleInteractable>().hoverEntered.AddListener(
                (HoverEnterEventArgs args) =>
                {
                    //toggle game object (parent) on and off
                    for (int i = 0; i < ToggleThis.Count; i++)
                    {
                        ToggleThis[i].SetActive(!ToggleThis[i].gameObject.activeSelf);
                    }
                }
                );
        }
    }
}
