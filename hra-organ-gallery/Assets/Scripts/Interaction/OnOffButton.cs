using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HRAOrganGallery
{
    public class OnOffButton : MonoBehaviour
    {
        [SerializeField] private List<GameObject> ToggleThis;

        private void OnTriggerEnter(Collider other)
        {
            //toggle game object (parent) on and off
            for (int i = 0; i < ToggleThis.Count; i++)
            {
                ToggleThis[i].SetActive(!ToggleThis[i].gameObject.activeSelf);
            }
        }
    }
}
