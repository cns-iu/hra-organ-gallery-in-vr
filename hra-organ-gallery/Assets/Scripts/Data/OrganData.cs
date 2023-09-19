using HRAOrganGallery;
using HRAOrganGallery.Assets.Scripts.Scene;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Data
{
    public enum BodySystem { undefined, integumentary, nervous, respiratory, cardio, digestive, musculoskeletal, lymphatic, urinary, fetal, reproductive }

    public class OrganData : MonoBehaviour, IApiSettable
    {
        [field: SerializeField]
        public string ReferenceOrgan { get; set; }

        [field: SerializeField]
        public string RepresentationOf { get; set; }

        [field: SerializeField]
        public string SceneGraph { get; set; }

        [field: SerializeField]
        public string Sex { get; set; }

        [field: SerializeField]
        public BodySystem BodySystem { get; set; }

        [field: SerializeField]
        public Vector3 DefaultPosition { get; set; }

        [field: SerializeField]
        public Vector3 DefaultPositionExtruded { get; set; }

        [field: SerializeField]
        public string Tooltip { get; set; }
        public void Init(Node node)
            => (ReferenceOrgan, RepresentationOf, SceneGraph, Tooltip)
            = (node.reference_organ, node.representation_of, node.scenegraph, node.tooltip);

        private string GetSex() {
            return SceneSetup.Instance.OrganSexMapping.pairs.First().sex;
        }
    }
}