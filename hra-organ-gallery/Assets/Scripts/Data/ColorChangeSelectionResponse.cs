using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HRAOrganGallery
{
    public class ColorChangeSelectionResponse : MonoBehaviour, IHoverResponse
    {
        [SerializeField] private Color _defaultColor;
        [SerializeField] private Color _hoverColor;

        public void OnHover(GameObject tissueBlock)
        {
            tissueBlock.GetComponent<Renderer>().material.color = _hoverColor;
        }

        public void OnHoverEnd(GameObject tissueBlock)
        {
            tissueBlock.GetComponent<Renderer>().material.color = _defaultColor;
        }

    }
}
