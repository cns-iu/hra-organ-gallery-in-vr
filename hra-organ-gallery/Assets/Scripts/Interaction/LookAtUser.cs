using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HRAOrganGallery
{
    public class LookAtUser : MonoBehaviour
    {
        private void Update()
        {
            transform.LookAt(Camera.main.transform.position);
        }
    }
}
