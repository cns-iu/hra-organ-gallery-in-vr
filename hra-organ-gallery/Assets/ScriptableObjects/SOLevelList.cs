using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HRAOrganGallery
{
    [
        CreateAssetMenu(
            fileName = "ElevatorLevelList",
            menuName = "ScriptableObjects/ElevatorLevelList",
            order = 1)
    ]
    public class SOLevelList : ScriptableObject
    {
        public List<SOLevelIndex> levels = new List<SOLevelIndex>();
    }
}
