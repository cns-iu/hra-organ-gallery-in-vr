using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace HRAOrganGallery
{
    public interface IHoverResponse
    {
        void OnHover(GameObject tissueBlock);
        void OnHoverEnd(GameObject tissueBlock);
    }
}
