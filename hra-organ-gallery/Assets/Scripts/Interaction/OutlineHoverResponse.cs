using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HRAOrganGallery
{
    public class OutlineHoverResponse : MonoBehaviour, IKeyboardHover
    {
        public void OnHoverEnter()
        {
            GetComponent<Outline>().enabled = true;
        }

        public void OnHoverExit()
        {
            GetComponent<Outline>().enabled = false;
        }
    }
}
