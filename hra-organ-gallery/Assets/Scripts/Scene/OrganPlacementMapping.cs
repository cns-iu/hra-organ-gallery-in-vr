using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HRAOrganGallery.Assets.Scripts.Scene
{
    /// <summary>
    /// A ScriptableObject to capture mappings between organ names and their position when loaded
    /// </summary>
    public class OrganPlacementMapping : ScriptableObject
    {
        public List<Mapping> mappings = new List<Mapping>();
    }

    [Serializable]
    public class Mapping {
        public string sceneGraphNode;
        public Vector3 assembledPosition;
    }


}
