using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HRAOrganGallery
{
    [
        CreateAssetMenu(
            fileName = "ElevatorLevel",
            menuName = "ScriptableObjects/ElevatorLevel",
            order = 1)
    ]
    public class SOLevelIndex : ScriptableObject
    {
        public int index;

        public string label;

        public string scale;
    }
}
