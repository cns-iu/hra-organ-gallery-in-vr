using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HRAOrganGallery
{
    public class BarManager : MonoBehaviour
    {
        public static BarManager Instance;

        // Start is called before the first frame update
        void Start()
        {
        
        }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
