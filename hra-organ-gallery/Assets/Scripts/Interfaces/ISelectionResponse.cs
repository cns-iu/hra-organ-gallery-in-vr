using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HRAOrganGallery
{
    public interface ISelectionResponse
    {
        void OnSelect(GameObject tissueBlock);
    }
}
