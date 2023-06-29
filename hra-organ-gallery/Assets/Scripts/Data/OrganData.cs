using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Data
{
    public class OrganData : MonoBehaviour
    {
        [field: SerializeField]
        public string RepresentationOf { get; set; }

        [field: SerializeField]
        public string SceneGraph { get; set; }

        [field: SerializeField]
        public string DonorSex { get; set; }

        [field: SerializeField]
        public BodySystem BodySystem { get; set; }

        [field: SerializeField]
        public Vector3 DefaultPosition { get; set; }

        [field: SerializeField]
        public Vector3 DefaultPositionExtruded { get; set; }

        [field: SerializeField]
        public string tooltip { get; set; }
    }
}