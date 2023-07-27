using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HRAOrganGallery
{
    public class HighlightSelectionResponse : MonoBehaviour, IHoverResponse
    {
        public void OnHover(GameObject tissueBlock)
        {
            tissueBlock.GetComponent<Outline>().enabled = true;
        }

        public void OnHoverEnd(GameObject tissueBlock)
        {
            tissueBlock.GetComponent<Outline>().enabled = false;
        }
    }
}
