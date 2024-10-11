using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HRAOrganGallery
{
    public class ElevatorLevelManager : MonoBehaviour
    {
        public static ElevatorLevelManager Instance;

        public SOLevelList LevelList => _levelList;

        [SerializeField]
        private SOLevelList _levelList;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
                return;
            }

            Instance = this;
        }
    }
}
