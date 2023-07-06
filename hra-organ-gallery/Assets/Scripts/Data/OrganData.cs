using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Data
{
    public class OrganData : MonoBehaviour, IApiSettable
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
        public string Tooltip { get; set; }
        public void Init(Node node)
            => (RepresentationOf, SceneGraph, Tooltip)
            = (node.representation_of, node.scenegraph, node.tooltip);
    }
}