using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HRAOrganGallery
{
    /// <summary>
    /// An interface describing a hover response for an XR interactable
    /// </summary>
    public interface IKeyboardHover
    {
        void OnHoverEnter();
        void OnHoverExit();
    }
}
