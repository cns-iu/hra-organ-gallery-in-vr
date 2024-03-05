using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HRAOrganGallery
{
    public class SetMeshRendererOnNodeName : MonoBehaviour
    {
        private void Awake()
        {
            if (!gameObject.name.Contains("_KEEP")) GetComponent<MeshRenderer>().enabled = false;
        }
    }
}
