using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace HRAOrganGallery
{
    public class ScaleSelf : MonoBehaviour
    {
        [SerializeField] private Vector3 newScale = new Vector3(.15f, .15f, .15f);

        private void Awake()
        {
            transform.localScale = newScale;
        }
    }
}
