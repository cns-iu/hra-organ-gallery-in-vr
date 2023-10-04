using HRAOrganGallery;
using HRAOrganGallery.Assets.Scripts.Scene;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Data
{

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
        public string Tooltip { get; set; }

        [field: SerializeField]
        public float Opacity { get; set; }

        [field: SerializeField]
        public Vector3 DefaultPosition { get; set; }

        public void Init(Node node)
        {
            ReferenceOrgan = Shared.Utils.CleanReferenceOrganName(node.reference_organ);
            RepresentationOf = node.representation_of;
            SceneGraph = node.scenegraph;
            Tooltip = node.tooltip;
            Sex = GetOrganSex(ReferenceOrgan, SceneSetup.Instance.OrganSexMapping);
            Opacity = node.opacity;
        }

        private string GetOrganSex(string referenceOrgan, OrganSexMapping mapping)
        {
            try
            {
                return mapping.pairs.First(o => o.reference_organ.Replace("\"", "") == referenceOrgan).sex.Replace("\"", "");
            }
            catch
            {
                if (referenceOrgan.Contains("VHF"))
                {
                    return "Female";
                }
                else
                {
                    return "Male";
                }
            }
        }
    }
}